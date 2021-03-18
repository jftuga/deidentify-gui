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

namespace deidentify_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public string textOriginal
        {
            get
            {
                return "In tennis, Naomi Osaka wins the women's singles and Novak Djokovic wins the men's singles at the Australian Open (both winners pictured). Naomi Osaka defeated Jennifer Brady in the final, 6–4, 6–3 to win the Women's Singles tennis title at the 2021 Australian Open. This was Osaka's second Australian Open title and her fourth Grand Slam tournament singles title. At this event Osaka extended her winning streak, dating back to the 2020 Cincinnati Open, to 21 matches. Osaka dropped just one set the entire tournament, to Garbiñe Muguruza in the fourth round; Osaka also saved two match points in this fourth round match against Muguruza. Osaka became the first female player to win four Grand Slam singles titles since Maria Sharapova in 2012, and the youngest since Justine Henin won the 2005 French Open. Osaka became the third player in the Open Era, after Monica Seles and Roger Federer, to win their first four Grand Slam finals. Sofia Kenin was the defending champion, but she lost to Kaia Kanepi in the second round. Her loss to Kanepi was the earliest by a women's defending champion at the Australian Open since two-time defending champion Jennifer Capriati lost in the opening round in 2003. Mayar Sherif became the first Egyptian woman to win a match in a Grand Slam tournament main draw. Serena Williams equalled Chris Evert's all-time record of reaching 54 Grand Slam quarterfinals. Hsieh Su-wei became the first female Taiwanese player to make a Grand Slam singles quarterfinal. At 35 years old, she is also the oldest player to debut in a Grand Slam quarterfinal.";
            }
        }

        public string textDeidentified
        {
            get
            {
                return "In tennis, Naomi Osaka wins the women's singles and Novak Djokovic wins the men's singles at the Australian Open (both winners pictured). Naomi Osaka defeated Jennifer Brady in the final, 6–4, 6–3 to win the Women's Singles tennis title at the 2021 Australian Open. This was Osaka's second Australian Open title and her fourth Grand Slam tournament singles title. At this event Osaka extended her winning streak, dating back to the 2020 Cincinnati Open, to 21 matches. Osaka dropped just one set the entire tournament, to Garbiñe Muguruza in the fourth round; Osaka also saved two match points in this fourth round match against Muguruza. Osaka became the first female player to win four Grand Slam singles titles since Maria Sharapova in 2012, and the youngest since Justine Henin won the 2005 French Open. Osaka became the third player in the Open Era, after Monica Seles and Roger Federer, to win their first four Grand Slam finals. Sofia Kenin was the defending champion, but she lost to Kaia Kanepi in the second round. Her loss to Kanepi was the earliest by a women's defending champion at the Australian Open since two-time defending champion Jennifer Capriati lost in the opening round in 2003. Mayar Sherif became the first Egyptian woman to win a match in a Grand Slam tournament main draw. Serena Williams equalled Chris Evert's all-time record of reaching 54 Grand Slam quarterfinals. Hsieh Su-wei became the first female Taiwanese player to make a Grand Slam singles quarterfinal. At 35 years old, she is also the oldest player to debut in a Grand Slam quarterfinal.";
            }
        }
    }
}
