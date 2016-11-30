using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EmprestimoUWP.Models;

namespace EmprestimoUWP.Migrations
{
    [DbContext(typeof(AppEmprestimo))]
    partial class AppEmprestimoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("EmprestimoUWP.Models.Contato", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Nome");

                    b.Property<string>("Telefone");

                    b.HasKey("Id");

                    b.ToTable("Contatos");
                });

            modelBuilder.Entity("EmprestimoUWP.Models.Emprestimo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ContatoId");

                    b.Property<DateTime>("DataEmprestimo");

                    b.Property<DateTime>("DataPrevDev");

                    b.Property<string>("Descricao");

                    b.Property<bool>("Devolvido");

                    b.Property<int>("IdContato");

                    b.HasKey("Id");

                    b.HasIndex("ContatoId");

                    b.ToTable("Emprestimos");
                });

            modelBuilder.Entity("EmprestimoUWP.Models.Emprestimo", b =>
                {
                    b.HasOne("EmprestimoUWP.Models.Contato", "Contato")
                        .WithMany()
                        .HasForeignKey("ContatoId");
                });
        }
    }
}
