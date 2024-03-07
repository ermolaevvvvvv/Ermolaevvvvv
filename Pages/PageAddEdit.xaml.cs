using Ermolaev.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
	/// Логика взаимодействия для PageAddEdit.xaml
	/// </summary>
	public partial class PageAddEdit : Page
	{
		private Postavki _postavki = new Postavki();
		public PageAddEdit(Postavki selectedPostavki)
		{
			InitializeComponent();
			if(selectedPostavki != null)
				_postavki = selectedPostavki;
			DataContext = _postavki;

			CmbProd.ItemsSource = ErmolaevEntities.GetContext().Produktsiya.ToList();
			CmbProd.SelectedValuePath = "id_produkta";
			CmbProd.DisplayMemberPath = "nazvanie_produktsii";

			CmbPred.ItemsSource = ErmolaevEntities.GetContext().Selskoe_predpriyatie.ToList();
			CmbPred.SelectedValuePath = "id_predpriyatiya";
			CmbPred.DisplayMemberPath = "nazvanie_predpriyatiya";
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_postavki.sebestoimost))
                errors.AppendLine("Укажите себестоимость продукта!");
            if (_postavki.obem < 0)
                errors.AppendLine("Укажите объём продукта!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_postavki.id_postavki == 0)
                ErmolaevEntities.GetContext().Postavki.Add(_postavki);
            try
            {
                ErmolaevEntities.GetContext().SaveChanges();
                MessageBox.Show("Инфорация сохранена успешно!");
                Classes.ClassFrame.frmObj.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Classes.ClassFrame.frmObj.GoBack();
        }
    }
}
