using EmprestimoUWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EmprestimoUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ListarContatos();
            dataEmpre.Date = DateTime.Now;
            dataDevo.Date = DateTime.Now;
        }

        private void ListarContatos()
        {
            AppEmprestimo db = new AppEmprestimo();
            if (db.Contatos.Count() != 0)
            {
                lvContatos.ItemsSource = null;
                lvContatos.ItemsSource = db.Contatos.ToList();
            }
        }

        private void ListarEmprestados()
        {
            AppEmprestimo db = new AppEmprestimo();
            if (db.Emprestimos.Count() != 0)
            {
                lvEmprestados.ItemsSource = null;
                foreach (Emprestimo emp in db.Emprestimos)
                {
                    emp.Contato = db.Contatos.Where(k => k.Id == Convert.ToInt32(emp.Contato.Id)).Single();
                    db.Emprestimos.Update(emp);
                }
                db.SaveChanges();
                lvEmprestados.ItemsSource = db.Emprestimos.ToList();
            }
        }

        private void PopularCB()
        {
            AppEmprestimo db = new AppEmprestimo();
            if (db.Contatos.Count() != 0)
            {
                cbContato.ItemsSource = db.Contatos.ToList();
                cbContato.SelectedValuePath = "Id";
                cbContato.DisplayMemberPath = "Nome";
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            pItemEdit.IsEnabled = true;
            pivotEmp.SelectedItem = pItemEdit;
            Contato obj = (Contato)lvContatos.SelectedItem;
            txtEditNome.Text = obj.Nome;
            txtEditEmail.Text = obj.Email;
            txtEditTel.Text = obj.Telefone;
        }

        private void btnInsEmp_Click(object sender, RoutedEventArgs e)
        {
            AppEmprestimo db = new AppEmprestimo();
            Contato contato = db.Contatos.Where(k => k.Id == Convert.ToInt32(cbContato.SelectedValue)).Single();
            Emprestimo emp = new Emprestimo
            {
                Descricao = txtDesc.Text,
                Contato = contato,
                IdContato = contato.Id,
                DataEmprestimo = Convert.ToDateTime(dataEmpre.Date),
                DataPrevDev = Convert.ToDateTime(dataDevo.Date),
                Devolvido = false
            };
            db.Emprestimos.Add(emp);
            db.SaveChanges();
            ListarEmprestados();
        }

        private void btnNDevolvido_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDevolvido_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInserirCont_Click(object sender, RoutedEventArgs e)
        {
            AppEmprestimo db = new AppEmprestimo();
            Contato c = new Contato
            {
                Nome = txtNome.Text,
                Email = txtEmail.Text,
                Telefone = txtTel.Text
            };
            db.Contatos.Add(c);
            db.SaveChanges();
            ListarContatos();
        }

        private void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            AppEmprestimo db = new AppEmprestimo();
            Contato obj = (Contato)lvContatos.SelectedItem;
            db.Contatos.Remove(obj);
            db.SaveChanges();
            ListarContatos();
        }

        private void btnEditCont_Click(object sender, RoutedEventArgs e)
        {
            Contato obj = (Contato)lvContatos.SelectedItem;
            AppEmprestimo db = new AppEmprestimo();

            Contato c = new Contato
            {
                Id = obj.Id,
                Nome = txtEditNome.Text,
                Email = txtEditEmail.Text,
                Telefone = txtEditTel.Text
            };
            db.Contatos.Update(c);
            db.SaveChanges();
            ListarContatos();

            txtEditNome.Text = "";
            txtEditEmail.Text = "";
            txtEditTel.Text = "";
            pItemEdit.IsEnabled = false;
        }
    }
}
