using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Independentsoft.Pst;
using System.IO;
using System.Threading;

namespace PST_Reader
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        //Var globale pour le chemin du PST
        string pathPST;

        private async void OuvrirPST()
        {
            //Ouvre boite de dialogue pour récupérer le PST
            OpenFileDialog PST = new OpenFileDialog();
            PST.Filter = "Fichier Outlook (.pst)|*.pst";

            //Si on valide le fichier sélectionné
            if (PST.ShowDialog() == DialogResult.OK)
            {
                //Path du PST
                pathPST = PST.FileName;
                //Affiche le Path dans la textbox
                txtPSTLocation.Text = pathPST;
                //Nouvel objet de type PSTFile
                PstFile PSTFile = new PstFile(pathPST);
                //On récupère le contenu du dossier contact
                Folder contacts = PSTFile.MailboxRoot.GetFolder("Contacts");
                //On vérifie qu'il ne soit pas vide

                if (contacts != null)
                {
                    //listContacts.Items.Clear();
                    //On créer une liste avec les contacts
                    IList<Item> items = contacts.GetItems();
                    for (int i = 0; i < items.Count; i++)
                    {
                        //Si l'item de la liste est bien un objet de type contact on l'affiche
                        if (items[i] is Contact && items != null)
                        {
                            Contact contact = (Contact)items[i];
                            //Remplissage de la listview
                            //Prénom
                            ListViewItem dataLst = listContacts.Items.Add(contact.GivenName);
                            //Nom
                            dataLst.SubItems.Add(contact.Surname);
                            //Nom affiché dans la liste de contact
                            dataLst.SubItems.Add(contact.DisplayName);
                            //Adresse principale
                            dataLst.SubItems.Add(contact.Email1Address);
                            //Adresse secondaire
                            dataLst.SubItems.Add(contact.Email2Address);
                            //Icone
                            dataLst.ImageIndex = 0;

                            toolStripStatusLabel1.Text = String.Format("Status : {0}", "Importation...");
                        }
                        
                    }

                    toolStripStatusLabel1.Text = String.Format("Status : {0}", "Importé avec succès!");
                }
            }
        }

        private void btnFileDialog_Click(object sender, EventArgs e)
        {
            OuvrirPST();
        }

        #region exportation

        //Exportation en .EML
        private void ExportToEML()
        {
            MessageBox.Show("Attention les fichiers .eml sont exportable sous Thunderbird grâce à cette extension : https://addons.mozilla.org/en-US/thunderbird/addon/importexporttools/", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Information);

            PstFile PSTFile = new PstFile(pathPST);
            IList<Folder> dossiers = PSTFile.MailboxRoot.GetFolders(true);
            Dictionary<long, string> parents = new Dictionary<long, string>();

            //Ou enregistrer les .eml
            FolderBrowserDialog exportPST = new FolderBrowserDialog();

            //Si on valide
            if (exportPST.ShowDialog() == DialogResult.OK)
            {
                //Paths
                string parentFolderPath = Path.GetFullPath(exportPST.SelectedPath);
                string currentFolderPath = parentFolderPath + @"\" + PSTFile.MailboxRoot.DisplayName;

                //Création du répertoire
                Directory.CreateDirectory(currentFolderPath);
                parents.Add(PSTFile.MailboxRoot.Id, currentFolderPath);

                new Thread(() =>
                {
                    Invoke((MethodInvoker)delegate
                    {
                        toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                        //progressWork.Style = ProgressBarStyle.Marquee;
                    });

                    Thread.CurrentThread.IsBackground = true;
                    for (int i = 0; i < dossiers.Count; i++)
                    {
                        Folder currentDossier = dossiers[i];

                        parentFolderPath = (string)parents[currentDossier.ParentId];
                        currentFolderPath = parentFolderPath + @"\" + currentDossier.DisplayName;

                        Directory.CreateDirectory(currentFolderPath);
                        parents.Add(currentDossier.Id, currentFolderPath);

                    }

                    for (int i = 0; i < dossiers.Count; i++)
                    {

                        for (int k = 0; k < dossiers[i].ChildrenCount; k += 100)
                        {
                            IList<Item> items = dossiers[i].GetItems(k, k + 100);


                            for (int m = 0; m < items.Count; m++)
                            {
                                string parentFolder = (string)parents[items[m].ParentId];
                                string fileName = GetFileName(items[m].Subject);
                                string filePath = parentFolder + @"\" + fileName;

                                if (filePath.Length > 244)
                                {
                                    filePath = filePath.Substring(0, 244);
                                }

                                filePath = filePath + "-" + items[m].Id + ".eml";
                                items[m].GetMessageFile().ConvertToMimeMessage().Save(filePath);

                                //Invoke car le controle à été créé sur un autre thread
                                Invoke((MethodInvoker)delegate
                                {
                                    toolStripStatusLabel1.Text = String.Format("Status : Traitement de {0}", fileName);
                                    if (i >= items.Count)
                                    {
                                        toolStripStatusLabel1.Text = String.Format("Status : {0}", "Inactif");
                                        //lblNomDossiers.Text = "Terminé !";
                                        //progressWork.Style = ProgressBarStyle.Blocks;
                                        
                                    }
                                });


                            }
                        }
                    }

                    //Invoke car le controle à été créé sur un autre thread
                    Invoke((MethodInvoker)delegate
                    {
                        toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                    });
                    

                    MessageBox.Show("Vos fichiers sont disponnible dans le répertoire suivant : " + parentFolderPath, "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }).Start();

            }
        }
                
        //Récupération du nom
        private string GetFileName(string subject)
        {
            if (subject == null || subject.Length == 0)
            {
                string fileName = "Pas de sujet";
                return fileName;
            }
            else
            {
                string fileName = "";

                for (int i = 0; i < subject.Length; i++)
                {
                    if (subject[i] > 31 && subject[i] < 127)
                    {
                        fileName += subject[i];
                    }
                }
                //Remplace les caractère interdits
                fileName = fileName.Replace("\\", "_");
                fileName = fileName.Replace("/", "_");
                fileName = fileName.Replace(":", "_");
                fileName = fileName.Replace("*", "_");
                fileName = fileName.Replace("?", "_");
                fileName = fileName.Replace("\"", "_");
                fileName = fileName.Replace("<", "_");
                fileName = fileName.Replace(">", "_");
                fileName = fileName.Replace("|", "_");

                return fileName;
            }
        }

        #endregion


        //Si aucun chemin n'est présent dans la txtbox
        private void TxtPSTLocation_TextChanged(object sender, EventArgs e)
        {
            if (txtPSTLocation.Text == "")
            {
                btnExport.Enabled = false;
            }
            else
            {
                btnExport.Enabled = true;
            }
        }

        //Titre de la form principale
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("PST Reader - Utilisé par : {0}", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        }

        //Menu supérieur
        #region Menu supérieur

        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Simule l'appuie sur le btn parcourir 
            btnFileDialog_Click(sender, e);
        }

        private void fermerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Ferme la form
            this.Close();
        }

        #endregion

        //Bouton dans le main
        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportToEML();
        }

        //Bouton en dessous de la listview
        private void exportCSVBtn_Click(object sender, EventArgs e)
        {
            ExportToCSV();
        }

        //Méthode pour l'exportation en CSV
        private async void ExportToCSV()
        {
            using (SaveFileDialog saveCSV = new SaveFileDialog() { Filter = "CSV|*.csv", ValidateNames = true })
            {
                if (saveCSV.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(new FileStream(saveCSV.FileName, FileMode.Create), Encoding.UTF8))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Prénom, Nom, Nom affiché, , Adresse mail, Mail secondaire, ");
                        foreach (ListViewItem item in listContacts.Items)
                        {
                            sb.AppendLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text, 
                                " ", item.SubItems[3].Text, item.SubItems[4].Text," "));
                        };
                        await sw.WriteLineAsync(sb.ToString());

                        MessageBox.Show("Contacts exportés", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        sw.Close();
                    }
                }
            }
        }

        //A propos (Menu)
        private void aProposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About APropos = new About();
            APropos.ShowDialog();
        }

        //Aide (Menu)
        private void aideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help Aide = new Help();
            Aide.ShowDialog();
        }
    }
}
