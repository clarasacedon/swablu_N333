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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace swablu_N333
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer dt1;
        Storyboard volar;
        Storyboard comer;
        Storyboard shiny;
        Storyboard atacado;
        Storyboard ataque;
        Storyboard sinEnergia;
        Storyboard sinVida;

        public MainWindow()
        {
            InitializeComponent();

            //Activar componentes
            this.imgPocima.IsEnabled = true;
            this.imgComida.IsEnabled = true;
            this.imgAtaque.IsEnabled = true;
            this.imgShiny.IsEnabled = true;
            this.Swablu.IsEnabled = true;

            //Asociar los storyboards
            this.volar = (Storyboard)this.Resources["volar"];
            this.comer = (Storyboard)this.Resources["comer"];
            this.shiny = (Storyboard)this.Resources["shiny"];
            this.atacado = (Storyboard)this.Resources["atacado"];
            this.ataque = (Storyboard)this.Resources["ataque"];
            this.sinEnergia = (Storyboard)this.Resources["sinEnergia"];
            this.sinVida = (Storyboard)this.Resources["sinVida"];

            //Comenzar reloj
            this.volar.Begin();
            dt1 = new DispatcherTimer();
            dt1.Interval = TimeSpan.FromSeconds(2);
            dt1.Tick += new EventHandler(reloj);
            dt1.Start();
        }

        private void reloj(object sender, EventArgs e)
        {
            if(this.pbVida.Value == 0)
            {
                fin();
                MessageBoxResult resultado = MessageBox.Show("SWABLU #333 - Derrotado\n" +
                    "¿Cerrar la aplicación?", "Fin", 
                    MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (resultado == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                if(pbComida.Value != 0)
                {
                    this.pbComida.Value -= 2;
                }
                
                this.pbVida.Value -= 2;

                if(this.pbVida.Value <= 30)
                {
                    energiaBaja();
                }
                else
                {
                    this.sinEnergia.Stop();
                }

                if (this.pbFuerza.Value != 0)
                {
                    this.pbFuerza.Value -= 2;
                } 
                else if (this.pbFuerza.Value == 0 || this.pbComida.Value == 0)
                {
                    this.pbVida.Value -= 5;
                }
            }
        }

        //RECARGAR VIDA Y FUERZA
        private void imgPocima_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.pbVida.Value += 10;
            this.pbFuerza.Value += 10;
        }

        //COMER
        private void imgComida_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.comer.Completed += new EventHandler(finComer);

            this.comer.Stop();
            this.volar.Stop();
            this.atacado.Stop();
            this.ataque.Stop();
            this.comer.Begin();

            this.pbComida.Value += 10;
        }

        private void finComer(object sender, EventArgs e)
        {
            this.comer.Stop();
            this.volar.Stop();
            this.atacado.Stop();
            this.ataque.Stop();
            this.volar.Begin();
        }

        //SHINY
        private void imgShiny_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.pbVida.Value >= 50 && this.pbFuerza.Value >= 50)
            {
                this.shiny.Stop();
                this.comer.Stop();
                this.atacado.Stop();
                this.ataque.Stop();
                this.shiny.Begin();
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show("No tengo suficiente energía", "ENERGIA BAJA",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //ATACADO
        private void Swablu_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.atacado.Completed += new EventHandler(finAtacado);

            this.volar.Stop();
            this.comer.Stop();
            this.shiny.Stop();
            this.ataque.Stop();
            this.atacado.Begin();
        }

        private void finAtacado(object sender, EventArgs e)
        {
            this.volar.Begin();
            this.pbFuerza.Value -= 5;
        }

        //ATACAR
        private void imgAtaque_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ataque.Completed += new EventHandler(finAtaque);

            this.ataque.Stop();
            this.volar.Stop();
            this.comer.Stop();
            this.atacado.Stop();
            this.ataque.Begin();
        }

        private void finAtaque(object sender, EventArgs e)
        {
            this.volar.Begin();
        }

        //QUEDA POCA VIDA
        private void energiaBaja()
        {
            this.comer.Completed += new EventHandler(finPocaEnergia);

            this.volar.Stop();
            this.comer.Stop();
            this.shiny.Stop();
            this.atacado.Stop();
            this.ataque.Stop();

            this.sinEnergia.Begin();
        }

        private void finPocaEnergia(object sender, EventArgs e)
        {
            this.volar.Begin();
        }

        // SIN VIDA
        private void fin()
        {
            dt1.Stop();

            this.volar.Stop();
            this.comer.Stop();
            this.shiny.Stop();
            this.atacado.Stop();
            this.ataque.Stop();
            this.sinEnergia.Stop();

            this.sinVida.Begin();

            this.imgPocima.IsEnabled = false;
            this.imgComida.IsEnabled = false;
            this.imgAtaque.IsEnabled = false;
            this.imgShiny.IsEnabled = false;
            this.Swablu.IsEnabled = false;
        }
    }
}
