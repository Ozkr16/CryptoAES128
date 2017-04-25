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
using AES256;
using Utilities;

namespace CryptoAES
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static char[] ValidHexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

		public MainWindow()
		{
			InitializeComponent();
		}

		private void BotonEncriptar_Click(object sender, RoutedEventArgs e)
		{
			String llave = TextboxLlave.Text;
			String nombreDeArchivo = TextboxNombreArchivo.Text;
			String textoEncriptar = TextboxTextoEncriptar.Text;
			bool wasSucess = false;


			if (llave == null || llave.Length != 32 
				|| textoEncriptar == null || textoEncriptar.Length > 100
				|| nombreDeArchivo == null
				|| !llave.ToUpper().ToCharArray().All( x => ValidHexChars.Contains(x))) {
				MessageBox.Show("Revise los parametros de entrada.");
				return;
			}
				

			byte[] llaveComoBytes = Util.StringToByteArray(llave);
			var datosEncryptados = EncriptadorAES.EncriptarConAES256(llaveComoBytes, textoEncriptar);
			
			if (datosEncryptados != null) {
				wasSucess = Util.WriteByteArrayToFile("./" + nombreDeArchivo, datosEncryptados);
			}
			
			if (wasSucess) {
				MessageBox.Show("Encripcion correcta. El archivo se encuentra en la misma localizacion que este programa.");
			}
			else
			{
				MessageBox.Show("Encripcion Fallida. Revise los parametros de entrada.");
			}
		}

		private void BotonDesencriptar_Click(object sender, RoutedEventArgs e)
		{
			String llave = TextboxLlave.Text;
			String nombreDeArchivo = TextboxNombreArchivo.Text;

			if (llave == null || llave.Length != 32
			|| nombreDeArchivo == null
			|| !llave.ToUpper().ToCharArray().All( x => ValidHexChars.Contains(x)))
			{
				MessageBox.Show("Revise los parametros de entrada.");
				return;
			}

			var datosEncryptados = Util.ReadByteArrayFromFile("./" + nombreDeArchivo);

			var textoPlano = EncriptadorAES.DesencriptarConAES256(Util.StringToByteArray(llave), datosEncryptados);

			if (textoPlano == null)
			{
				MessageBox.Show("Encripcion Fallida. Revise los parametros de entrada.");
				return;
			}
			TextboxTextoEncriptar.Text = textoPlano;
		}
	}
}
