using EmprestimoUWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
            ListarEmprestados();
            dataEmpre.Date = DateTime.Now;
            dataDevo.Date = DateTime.Now;
            PopularCB();
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
                    emp.Contato = db.Contatos.Where(k => k.Id == Convert.ToInt32(emp.IdContato)).Single();
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

        private async void btnInsEmp_Click(object sender, RoutedEventArgs e)
        {
            AppEmprestimo db = new AppEmprestimo();
            Contato contato = db.Contatos.Where(k => k.Id == Convert.ToInt32(cbContato.SelectedValue)).Single();
            DateTimeOffset sourceTime1 = (DateTimeOffset)dataEmpre.Date;
            DateTime DataEmpre = sourceTime1.DateTime;
            DateTimeOffset sourceTime2 = (DateTimeOffset)dataDevo.Date;
            DateTime DataDevo = sourceTime2.DateTime;
            Emprestimo emp = new Emprestimo
            {
                Descricao = txtDesc.Text,
                Contato = contato,
                IdContato = contato.Id,
                DataEmprestimo = DataEmpre,
                DataPrevDev = DataDevo,
                DataDevolucao = new DateTime(),
                Devolvido = false
            };
            if (emp.DataPrevDev >= emp.DataEmprestimo)
            {
                db.Emprestimos.Add(emp);
                db.SaveChanges();
                ListarEmprestados();
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Selecione uma data de devolução válida!");
                await dialog.ShowAsync();
            }
        }

        private void btnNDevolvido_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnDevolvido_Click(object sender, RoutedEventArgs e)
        {
            Emprestimo obj = (Emprestimo)lvEmprestados.SelectedItem;
            if (obj.Devolvido == false)
            {
                if (DateTime.Now >= obj.DataPrevDev)
                {
                    AppEmprestimo db = new AppEmprestimo();
                    Emprestimo emp = new Emprestimo
                    {
                        Id = obj.Id,
                        IdContato = obj.IdContato,
                        Descricao = obj.Descricao,
                        Contato = obj.Contato,
                        DataEmprestimo = obj.DataEmprestimo,
                        DataPrevDev = obj.DataPrevDev,
                        Devolvido = true,
                        DataDevolucao = DateTime.Now
                    };
                    db.Emprestimos.Update(emp);
                    db.SaveChanges();
                    ListarEmprestados();
                }
                else
                {
                    MessageDialog dialog = new MessageDialog(
                        "O prazo para entrega ainda não foi atingido! \n Deseja marcar como 'devolvido' mesmo assim?"
                        );
                    dialog.Commands.Add(new UICommand("Sim") { Id = 0 });
                    dialog.Commands.Add(new UICommand("Não") { Id = 1 });
                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;

                    var result = await dialog.ShowAsync();
                    if ((int)result.Id == 0)
                    {
                        AppEmprestimo db = new AppEmprestimo();
                        Emprestimo emp = new Emprestimo
                        {
                            Id = obj.Id,
                            IdContato = obj.IdContato,
                            Descricao = obj.Descricao,
                            Contato = obj.Contato,
                            DataEmprestimo = obj.DataEmprestimo,
                            DataPrevDev = obj.DataPrevDev,
                            Devolvido = true,
                            DataDevolucao = DateTime.Now
                        };
                        db.Emprestimos.Update(emp);
                        db.SaveChanges();
                        ListarEmprestados();
                    }
                }
            }
            else
            {
                MessageDialog dialog = new MessageDialog("O objeto já foi devolvido!");
                await dialog.ShowAsync();
            }
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
            PopularCB();
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
            PopularCB();
        }
    }
}
