using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Data.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using System.Drawing.Drawing2D;

namespace ManejoDeTablas
{
    public partial class FrmEnvioCorreos : Form
    {
        List<EnvioCorreo> lista = new List<EnvioCorreo>();
        public FrmEnvioCorreos()
        {
            InitializeComponent();
            dgvTabla.DataSource = lista;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //LLENAR UN OBJETO CON LOS DATOS
            EnvioCorreo registro = new EnvioCorreo(
                cboEnviado.Text.Equals("NO")?false:true,
                txtCorreo.Text.ToLower(),
                txtNombre.Text.ToUpper(),
                double.Parse(txtCalificacion.Text)
                );

            Console.WriteLine(registro.Enviado);
            Console.WriteLine(cboEnviado.Text);

            //AÑADIR A LA LISTA
            lista.Add(registro);

            //ACTUALIZAR TABLA
            //dgvTabla.DataSource = null;
            //dgvTabla.DataSource = lista;

            CargarLista();

            //dgvTabla.Rows.Add(cboEnviado.SelectedItem, txtCorreo.Text, txtNombre.Text, txtCalificacion.Text);
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvTabla.SelectedRows.Count == 0) {
                MessageBox.Show("Selecciona una fila a eliminar");
            }
            else {
                String correo = dgvTabla.SelectedRows[0].Cells[1].Value.ToString();
                DialogResult respueste = MessageBox.Show("Esta a punto de eliminar al alumno " + correo + ", ¿Está seguro" +
                    "que desea continuar? ", "ELIMINACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respueste == DialogResult.Yes)
                {
                    lista.Remove(new EnvioCorreo() { Correo = correo });
                    CargarLista();
                }
            }
        }

        private void CargarLista()
        {
            dgvTabla.DataSource = null;
            dgvTabla.DataSource = lista;
            //dgvTabla.Columns[0]
            foreach (DataGridViewColumn column in dgvTabla.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            //fdFile.InitialDirectory = "c:\\";
            fdFile.Filter = "txt files (*.txt)|*.txt";
            fdFile.FilterIndex = 2;
            fdFile.RestoreDirectory = true;

            if (fdFile.ShowDialog() == DialogResult.OK)
            {
                //Read the contents of the file into a stream
                var fileStream = fdFile.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    String linea = reader.ReadLine();
                    while (linea != null)
                    {

                        //Console.WriteLine(linea);
                        String[] subs = linea.Split(',');

                        if(subs.Length == 4)
                            lista.Add(new EnvioCorreo(subs[0].ToLower() == "si" ? true : false, subs[1], subs[2], double.Parse(subs[3])));

                        else if( subs.Length == 3)
                            lista.Add(new EnvioCorreo(false, subs[0], subs[1], double.Parse(subs[2])));

                        linea = reader.ReadLine();
                    }
                    reader.Close();
                }
            }

            CargarLista();
                        
        }

        Thread correos;
        private void btnSendMail_Click(object sender, EventArgs e)
        {
            correos = new Thread(SendEmails);
            correos.Start();
            
        }

        public void SendEmails()
        {
            int index = 1;
            int total = lista.Count;
            String cuerpo;

            foreach (var mail in lista)
            {
                if (!mail.Enviado)
                {
                    cuerpo = rtxtbPlantilla.Text.Replace("<Alumno>", mail.Alumno);
                    cuerpo = rtxtbPlantilla.Text.Replace("<Correo>", mail.Correo);
                    cuerpo = rtxtbPlantilla.Text.Replace("<Calificación>", mail.Calificacion.ToString());
                    
                    try
                    {

                        MailMessage correo = new MailMessage
                        {
                            From = new MailAddress("guillermomataramos1234@gmail.com", "Guillermo")
                        };
                        correo.To.Add(mail.Correo);
                        correo.Subject = "Envio de calificaiones";
                        correo.Body = cuerpo;
                        correo.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 465)
                        {
                            Credentials = new NetworkCredential("guillermomataramos1234@gmail.com", "gpahjaekltxzaqxe"),
                            EnableSsl = true
                        };
                        smtp.Send(correo);
                        mail.Enviado = true;
                        index = index + 1;
                        lblEnviados.Text = "Enviando(" + index + "/" + total + ")";
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }

            }


            CargarLista();
        }

        private void FrmRegistroEliminacionColecciones_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void FrmRegistroEliminacionColecciones_FormClosing(object sender, FormClosingEventArgs e)
        {
                       
        }
    }
}
