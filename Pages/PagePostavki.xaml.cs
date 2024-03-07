using Ermolaev.Classes;
using System;
using System.Collections.Generic;
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

namespace Ermolaev.Pages
{
	/// <summary>
	/// Логика взаимодействия для PagePostavki.xaml
	/// </summary>
	public partial class PagePostavki : Page
	{
		public PagePostavki()
		{
			InitializeComponent();


			//dtgPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.ToList();

			CmbYear.ItemsSource = ErmolaevEntities.GetContext().Selskoe_predpriyatie.ToList();
			CmbYear.SelectedValuePath = "id_predpriyatiya";
			CmbYear.DisplayMemberPath = "nazvanie_predpriyatiya";

			CmbName.ItemsSource = ErmolaevEntities.GetContext().Produktsiya.ToList();
            CmbName.SelectedValuePath = "id_produkta";
            CmbName.DisplayMemberPath = "nazvanie_produktsii";
        }

		private void CmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int pred = int.Parse(CmbYear.SelectedValue.ToString());
			dtgPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.Where(x => x.id_predpriyatiya == pred).ToList();
		}
		private void BtnResetFiltr_Click(object sender, RoutedEventArgs e)
		{

			dtgPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.ToList();

		}

		private void BtnAdd_Click(object sender, RoutedEventArgs e)
		{
			Classes.ClassFrame.frmObj.Navigate(new Pages.PageAddEdit(null));
		}

        private void CmbName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int prod = int.Parse(CmbName.SelectedValue.ToString());
            dtgPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.Where(x => x.id_produkta == prod).ToList();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var Remove = dtgPostavki.SelectedItems.Cast<Postavki>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {Remove.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    ErmolaevEntities.GetContext().Postavki.RemoveRange(Remove);
                    ErmolaevEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    dtgPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnGoList_Click(object sender, RoutedEventArgs e)
        {
            Classes.ClassFrame.frmObj.Navigate(new Pages.PageListPostavki());
        }

        private void TxtSearchSum_TextChanged(object sender, TextChangedEventArgs e)
        {
			string search = TxtSearchSum.Text;
			dtgPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.Where(x => x.sebestoimost.Contains(search)).ToList();
		}

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                ErmolaevEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dtgPostavki.ItemsSource = ErmolaevEntities.GetContext().Postavki.ToList();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Classes.ClassFrame.frmObj.Navigate(new Pages.PageAddEdit((sender as Button).DataContext as Postavki));
        }
    }
}