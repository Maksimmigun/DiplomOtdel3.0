﻿using DiplomOtdel.Context;
using DiplomOtdel.ModelSQL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace DiplomOtdel.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageNavigate.xaml
    /// </summary>
    public partial class PageNavigate : Page
    {
        public PageNavigate()
        {
            InitializeComponent();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListMain.ItemsSource = dbcontext.db.MainInfo.ToList();
        }


        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPage());
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            MainInfo editrow = (MainInfo)ListMain.SelectedItem;
            if (editrow != null)
            {
                NavigationService.Navigate(new EditPage(editrow));
            }
        }

        private void ButtonMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            MainInfo morerow = (MainInfo)ListMain.SelectedItem;
            if (morerow != null)
            {
                NavigationService.Navigate(new MoreInfoPage(morerow));
            }
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListMain.ItemsSource = dbcontext.db.MainInfo.Where(item => item.Name.Contains(TextBoxSearch.Text) || item.EmploymentRecord.Specialization.Contains(TextBoxSearch.Text) || item.Surname.Contains(TextBoxSearch.Text)).ToList();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MainInfo deleteEmployee = (MainInfo)ListMain.SelectedItem;
            if (deleteEmployee != null)
            {
                if (MessageBox.Show("Вы точно хотите удалить выбранного работника?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    dbcontext.db.MainInfo.Remove(deleteEmployee);
                    dbcontext.db.SaveChanges();
                    Page_Loaded(null, null);
                    MessageBox.Show("Вы удалили выбранного работника", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали элемент", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Authoization());
        }



        private void ButtonWord_Click(object sender, RoutedEventArgs e)
        {
            {
                var word = new Word.Application();

                try
                {
                    var document = word.Documents.Add();
                    var paragrah = word.ActiveDocument.Paragraphs.Add();
                    var tableRange = paragrah.Range;
                    var serviceList = dbcontext.db.MainInfo.ToList();
                    var table = document.Tables.Add(tableRange, serviceList.Count, 7);
                    table.Borders.Enable = 1;
                    table.Cell(1, 1).Range.Text = "Имя";
                    table.Cell(1, 2).Range.Text = "Фамилия";
                    table.Cell(1, 3).Range.Text = "Отчество";
                    table.Cell(1, 4).Range.Text = "Зарплата";
                    table.Cell(1, 5).Range.Text = "Надбавка";
                    table.Cell(1, 6).Range.Text = "Последняя дата выдачи";
                    table.Cell(1, 7).Range.Text = "Подпись сотрудника";


                    int i = 2;
                    foreach (var item in serviceList)
                    {
                        table.Cell(i, 0).Range.Text = item.ID.ToString();
                        table.Cell(i, 1).Range.Text = item.Name;
                        table.Cell(i, 2).Range.Text = item.Surname;
                        table.Cell(i, 3).Range.Text = item.Patronymic;
                        table.Cell(i, 4).Range.Text = item.Salary.TheAmount;
                        table.Cell(i, 5).Range.Text = item.Salary.Allowance;
                        table.Cell(i, 6).Range.Text = Convert.ToString(item.Salary.LastIssueDate);
                        table.Cell(i, 7).Range.Text = "";
                        i++;
                    }
                    document.SaveAs2(@"E:\Зарплаты.docx");
                    document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                    word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                    MessageBox.Show("Сохранение прошло успешно в Word!", "Сохранено!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source + "Произошла ошибка. Возможно вы что-то делаете не правильно!", MessageBoxButton.OK, MessageBoxImage.Error);
                    word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                }
            }
        }

        private void ButtonSupport_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SupportPage());
        }

        private void ButtonWordT1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportNavigatePage());
        }
    }
}
