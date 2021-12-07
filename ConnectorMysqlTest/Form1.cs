using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace ConnectorMysqlTest
{
    public partial class Form1 : Form
    {

        MySqlConnection conexao;
        string data_source = "Server=localhost;user id=root;password=root;DATABASE=db_agenda";

        private int? idContatoSelecionado = null;
        public Form1()
        {
            InitializeComponent();
            ConfiguracaoListaContatos();
            carregarContatos();

        }

        private void ConfiguracaoListaContatos()
        {
            listContatos.View = View.Details;
            listContatos.LabelEdit = true;
            listContatos.AllowColumnReorder = true;
            listContatos.FullRowSelect = true;
            listContatos.GridLines = true;

            listContatos.Columns.Add("ID ", 30, HorizontalAlignment.Left);
            listContatos.Columns.Add("Nome ", 60, HorizontalAlignment.Left);
            listContatos.Columns.Add("Email ", 150, HorizontalAlignment.Left);
            listContatos.Columns.Add("Telefone ", 120, HorizontalAlignment.Left);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                //Criando conexão com MySQL
                conexao = new MySqlConnection(data_source);

                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;

                if (idContatoSelecionado == null)
                {
                    //insert
                    cmd.Prepare();

                    cmd.CommandText = "INSERT INTO contato(nome,email,telefone) " +
                                      " VALUES " +
                                      "(@nome, @email, @telefone) ";


                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);

                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Contato inserido!");
                    txtNome.Clear();
                    txtEmail.Clear();
                    txtTelefone.Clear();
                    carregarContatos();
                }
                else
                {
                    //update
                    cmd.Prepare();

                    cmd.CommandText = "UPDATE contato SET nome = @nome, email = @email, telefone = @telefone " +
                                      " WHERE id = @id ";

                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@id", idContatoSelecionado);

                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Contato atualizado!");
                    txtNome.Clear();
                    txtEmail.Clear();
                    txtTelefone.Clear();
                    idContatoSelecionado = null;
                    carregarContatos();
                }



            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                conexao = new MySqlConnection(data_source);

                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.Prepare();
                cmd.CommandText = "SELECT * FROM contato WHERE nome LIKE @q OR email LIKE @q OR id LIKE @q";


                cmd.Parameters.AddWithValue("@q", "%" + txtBuscar.Text + "%");


                MySqlDataReader reader = cmd.ExecuteReader();

                listContatos.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3)
                    };

                    listContatos.Items.Add(new ListViewItem(row));
                }


            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void carregarContatos()
        {
            try
            {

                conexao = new MySqlConnection(data_source);

                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.Prepare();
                cmd.CommandText = "SELECT * FROM contato ORDER BY id DESC";

                MySqlDataReader reader = cmd.ExecuteReader();

                listContatos.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3)
                    };

                    listContatos.Items.Add(new ListViewItem(row));
                }


            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void listContatos_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = listContatos.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                idContatoSelecionado = int.Parse(item.SubItems[0].Text);

                txtNome.Text = item.SubItems[1].Text;
                txtEmail.Text = item.SubItems[2].Text;
                txtTelefone.Text = item.SubItems[3].Text;


            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtNome.Clear();
            txtEmail.Clear();
            txtTelefone.Clear();
            idContatoSelecionado = null;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir o registro?",
                    "ops, tem certeza?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (conf == DialogResult.Yes)
                {
                    //Excluir contato
                    conexao = new MySqlConnection(data_source);

                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexao;

                    cmd.Prepare();

                    cmd.CommandText = "DELETE FROM contato WHERE id = @id ";

                    cmd.Parameters.AddWithValue("@id", idContatoSelecionado);

                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Contato exluido");
                    carregarContatos();
                }



            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir o registro?",
                    "ops, tem certeza?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (conf == DialogResult.Yes)
                {
                    //Excluir contato
                    conexao = new MySqlConnection(data_source);

                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexao;

                    cmd.Prepare();

                    cmd.CommandText = "DELETE FROM contato WHERE id = @id ";

                    cmd.Parameters.AddWithValue("@id", idContatoSelecionado);

                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Contato exluido");
                    carregarContatos();
                }



            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}
