using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
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
using static System.Net.Mime.MediaTypeNames;


namespace _2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /*

    + 1 Ошибка с опусканием вниз

    + 2 Ошибка если две плитки разного значения первая уходит в конец вторая остается

    3 Конец игры кнопки NEW GAME и Retry

    4 сохранение результатов в фаил

    5 load hight score из файла

    + 6 сделать победу при значении плитки 2048

    + 7 сделать плитки до 2048 (512 ,1024 , 2048)

    8 зробити нормальний чек пораски ( пораска йде навіть якщо ще є кроки) 

    */
    public partial class MainWindow : Window
    {
        int[,] map = new int[4, 4];
        int score = 0, hight_score = 0;

        public MainWindow()
        {
            InitializeComponent();
            start_Game();
        }

        private void start_Game()
        {
            Random random = new Random();
            //map[random.Next(0, 3), random.Next(0, 3)] = 2;
            //map[random.Next(0, 3), random.Next(0, 3)] = 2;

            map[0,1] = 2;
            map[0,2] = 2;
            map[0,3] = 4;

            //map[0, 0] = 2; 
            //map[0, 1] = 4; 
            //map[0, 2] = 8; 
            //map[0, 3] = 16; 
            //map[1, 0] = 32; 
            //map[1, 1] = 64;
            //map[1, 2] = 128;
            //map[1, 3] = 256;
            //map[2, 0] = 1024;
            //map[2, 1] = 1024;
            //map[2, 2] = 2048;

            loadGame();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key.ToString())
            {
                case "Left":
                    moveTileToLeft();
                    break;
                case "Right":
                    moveTileToRight();
                    break;
                case "Up":
                    moveTileUp();
                    break;
                case "Down":
                    moveTileDown();
                    break;

                default: return;
            }
        }

        private void moveTileUp()
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //потрібно щоб плитка не змінювалась два рази за один крок
                    bool numChanged = true;
                    // якщо плитки однакові  
                    if (map[i, j] == map[i - 1, j] && map[i, j] != 0 && map[i - 1, j] != 0)
                    {
                        map[i - 1, j] *= 2;
                        map[i, j] = 0;
                        numChanged = false;
                        int i1 = i - 1;
                        while (true)
                        {
                            if (i1 - 1 < 0 || (map[i1, j] != map[i1 - 1, j] && map[i1 - 1, j] != 0))
                                break;
                            else if (map[i1 - 1, j] == 0)
                            {
                                map[i1 - 1, j] = map[i1, j];
                                map[i1, j] = 0;
                            }
                            i1--;
                        }
                    }
                    //якщо плитки різні
                    if (map[i, j] != map[i - 1, j] && map[i - 1, j] != 0)
                    {
                        int i1 = i - 1;
                        while (true)
                        {
                            if (i1 - 1 < 0 || (map[i1, j] != map[i1 - 1, j] && map[i1 - 1, j] != 0))
                                break;
                            else if (map[i1 - 1, j] == 0)
                            {
                                map[i1 - 1, j] = map[i1, j];
                                map[i1, j] = 0;
                            }
                            else if (map[i1, j] == map[i1 - 1, j])
                            {
                                if (numChanged)
                                {
                                    map[i1 - 1, j] *= 2;
                                    map[i1, j] = 0;
                                    score += map[i1 - 1, j];
                                    numChanged = false;
                                }
                            }
                            i1--;
                        }
                    }
                    //якщо плитка вище дорівнює нулю
                    if (map[i, j] != map[i - 1, j] && map[i - 1, j] == 0)
                    {
                        map[i - 1, j] = map[i, j];
                        map[i, j] = 0;
                        int i1 = i - 1;

                        while (true)
                        {
                            if (i1 - 1 < 0 || (map[i1, j] != map[i1 - 1, j] && map[i1 - 1, j] != 0)) break;

                            if (map[i1 - 1, j] == 0)
                            {
                                map[i1 - 1, j] = map[i1, j];
                                map[i1, j] = 0;
                            }
                            else if (map[i1, j] == map[i1 - 1, j])
                            {
                                if (numChanged)
                                {
                                    map[i1 - 1, j] *= 2;
                                    map[i1, j] = 0;
                                    numChanged = false;    
                                    score += map[i1 - 1, j];
                                }
                            }
                            i1--;
                        }
                    }
                }
                loadGame();
            } 
            generateNewTile();
        }

        private void moveTileToLeft()
        {

            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    //потрібно щоб плитка не змінювалась два рази за один крок
                    bool numChanged = true;
                    // якщо плитки однакові  
                    if (map[i, j] == map[i , j - 1] && map[i, j] != 0 && map[i, j - 1] != 0)
                    {
                        map[i, j - 1] *= 2;
                        map[i, j] = 0;
                        numChanged = false;
                        int j1 = j - 1;          
                        while (true)
                        {
                            if (j1 - 1 < 0 || (map[i, j1] != map[i, j1 - 1] && map[i, j1 - 1] != 0))
                                break;
                            else if (map[i, j1 - 1] == 0)
                            {
                                map[i, j1 - 1] = map[i, j1];
                                map[i, j1] = 0;
                            }
                            j1--;
                        }
                    }
                    //якщо плитки різні
                    if (map[i, j] != map[i, j - 1] && map[i, j - 1] != 0)
                    {
                        int j1 = j - 1;
                        while (true)
                        {
                            if (j1 - 1 < 0 || (map[i, j1] != map[i, j1 - 1] && map[i, j1 - 1] != 0))
                                break;
                            else if (map[i, j1 - 1] == 0)
                            {
                                map[i, j1 - 1] = map[i, j1];
                                map[i, j1] = 0;
                            }
                            else if (map[i, j1] == map[i, j1 - 1])
                            {
                                if (numChanged)
                                {
                                    map[i, j1 - 1] *= 2;
                                    map[i, j1] = 0;
                                    numChanged = false;
                                    score += map[i, j1 - 1];
                                }
                            }
                            j1--;
                        }
                    }
                    //якщо плитка вище дорівнює нулю
                    if (map[i, j] != map[i, j - 1] && map[i , j - 1] == 0)
                    {
                        map[i, j - 1] = map[i, j];
                        map[i, j] = 0;
                        int j1 = j - 1;

                        while (true)
                        {
                            if (j1 - 1 < 0 || (map[i, j1] != map[i, j1 - 1] && map[i, j1 - 1] != 0)) break;

                            if (map[i, j1 - 1] == 0)
                            {
                                map[i, j1 - 1] = map[i, j1];
                                map[i, j1] = 0;
                            }
                            else if (map[i, j1] == map[i, j1 - 1])
                            {
                                if (numChanged)
                                {
                                    map[i, j1 - 1] *= 2;
                                    map[i, j1] = 0;
                                    numChanged = false;
                                    score += map[i, j1 - 1];
                                }
                            }
                            j1--;
                        }
                    }
                }
                loadGame();
            }
            generateNewTile();
        }

        private void moveTileToRight()
        {
           
            for (int i = 0; i < 4; i++)
            { 
                for (int j = 0; j < 3; j++)
                {
                    //потрібно щоб плитка не змінювалась два рази за один крок
                    bool numChanged = true;
                    // якщо плитки однакові  
                    if (map[i, j] == map[i, j + 1] && map[i, j] != 0 && map[i, j + 1] != 0)
                    {

                        map[i, j + 1] *= 2;
                        map[i, j] = 0;
                        numChanged = false;
                        int j1 = j + 1;

                        while (true)
                        {
                            if (j1 + 1 > 3 || (map[i, j1] != map[i, j1 + 1] && map[i, j + 1] != 0))
                                break;

                            if (map[i, j1 + 1] == 0)
                            {
                                map[i, j1 + 1] = map[i, j1];
                                map[i, j1] = 0;
                            }
                            j1++;
                        }
                    }
                    //якщо плитки різні
                    if (map[i, j] != map[i, j + 1] && map[i, j + 1] != 0)
                    {
                        int j1 = j + 1;
                        while (true)
                        {
                            if (j1 + 1 > 3 || (map[i, j1] != map[i, j1 + 1] && map[i, j1 + 1] != 0))
                                break;
                            else if (map[i, j1 + 1] == 0)
                            {
                                map[i, j1 + 1] = map[i, j1];
                                map[i, j1] = 0;
                            }
                            else if (map[i, j1] == map[i, j1 + 1])
                            {
                                if (numChanged)
                                {
                                    map[i, j1 + 1] *= 2;
                                    map[i, j1] = 0;
                                    numChanged = false;
                                    score += map[i, j1 + 1];
                                }
                            }
                            j1++;
                        }
                    }
                    //якщо плитка вище дорівнює нулю
                    if (map[i, j] != map[i, j + 1] && map[i, j + 1] == 0)
                    {
                        map[i, j + 1] = map[i, j];
                        map[i, j] = 0;
                        int j1 = j + 1;

                        while (true)
                        {
                            if (j1 + 1 > 3 || (map[i, j1] != map[i, j1 + 1] && map[i, j1 + 1] != 0)) break;

                            if (map[i, j1 + 1] == 0)
                            {
                                map[i, j1 + 1] = map[i, j1];
                                map[i, j1] = 0;
                            }
                            else if (map[i, j1] == map[i, j1 + 1])
                            {
                                if (numChanged)
                                {
                                    map[i, j1 + 1] *= 2;
                                    map[i, j1] = 0;
                                    numChanged = false;
                                    score += map[i, j1 + 1];
                                }
                            }
                            j1++;
                        }
                    }
                }
                loadGame();
            }
            generateNewTile();
        }
        
        private void moveTileDown() 
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //потрібно щоб плитка не змінювалась два рази за один крок
                    bool numChanged = true;
                    // якщо плитки однакові  
                    if (map[i, j] == map[i + 1, j] && map[i, j] != 0 && map[i + 1, j] != 0)
                    {

                        map[i + 1, j] *= 2;
                        map[i, j] = 0;
                        numChanged = false;
                        int i1 = i + 1;

                        while (true)
                        {
                            if (i1 + 1 > 3 || (map[i1, j] != map[i1 + 1, j] && map[i1, j] != 0))
                                break;

                            if (map[i1 + 1, j] == 0)
                            {
                                map[i1 + 1, j] = map[i1 + 1, j];
                                map[i1, j] = 0;
                            }
                            i1++;
                        }
                    }
                    //якщо плитки різні
                    if (map[i, j] != map[i + 1, j] && map[i + 1, j] != 0)
                    {
                        int i1 = i + 1;
                        while (true)
                        {
                            if (i1 + 1 > 3 || (map[i1, j] != map[i1 + 1, j] && map[i1 + 1, j] != 0))
                                break;
                            else if (map[i1 + 1, j] == 0)
                            {
                                map[i1 + 1, j] = map[i1, j];
                                map[i1, j] = 0;
                            }
                            else if (map[i1, j] == map[i1 + 1, j])
                            {
                                if (numChanged)
                                {
                                    map[i1 + 1, j] *= 2;
                                    map[i1, j] = 0;
                                    numChanged = false;
                                    score += map[i1 + 1, j];
                                }
                            }
                            i1++;
                        }
                    }
                    //якщо плитка вище дорівнює нулю
                    if (map[i, j] != map[i + 1, j] && map[i + 1, j] == 0)
                    {
                        map[i + 1, j] = map[i, j];
                        map[i, j] = 0;
                        int i1 = i + 1;

                        while (true)
                        {
                            if (i1 + 1 > 3 || (map[i1, j] != map[i1 + 1, j] && map[i1 + 1, j] != 0)) break;

                            if (map[i1 + 1, j] == 0)
                            {
                                map[i1 + 1, j] = map[i1, j];
                                map[i1, j] = 0;
                            }
                            else if (map[i1, j] == map[i1 + 1, j])
                            {
                                if (numChanged)
                                {
                                    map[i1 + 1, j] *= 2;
                                    map[i1, j] = 0;
                                    numChanged = false;
                                    score += map[i1 + 1, j];
                                }
                            }
                            i1++;
                        }
                    }
                }
                loadGame();
            }
            generateNewTile();
        }

        private void ClearUniformGrid()
        {
            int length = Ug_Map.Children.Count;
            for (int i = 0; i < length; i++)
            {
                Ug_Map.Children.Remove(Ug_Map.Children[0]);
            }
        }

        private void generateNewTile()
        {
            Random random = new Random();
            while (true)
            {
                int x = random.Next(0, 4);
                int y = random.Next(0, 4);

                if (map[x, y] != 0)
                    continue;

                map[x, y] = 2;
                break;
            }
            loadGame();
        }

        private void loadGame()
        {           
            ClearUniformGrid();
            txt_Score.Text = score.ToString();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (map[i, j] == 0)
                    {
                        Border brd = new Border();
                        brd.Style = (Style)this.Resources["border_style"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 2)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "2";
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_2"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 4)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "4";
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_4"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 8)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "8";
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_8"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 16)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "16";
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_16"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 32)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "32";
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_32"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 64)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "64";
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_64"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 128)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "128";
                        txt.Style = (Style)this.Resources["txt_style"];
                        txt.Foreground = Brushes.White;
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_128"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 256)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "256";
                        txt.Foreground = Brushes.White;
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_256"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 512)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "512";
                        txt.Foreground = Brushes.White;
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_512"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 1024)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "1024";
                        txt.Foreground = Brushes.White;
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_1024"];
                        Ug_Map.Children.Add(brd);
                    }
                    else if (map[i, j] == 2048)
                    {
                        Border brd = new Border();
                        TextBlock txt = new TextBlock();
                        txt.Text = "2048";
                        txt.Foreground = Brushes.White;
                        txt.Style = (Style)this.Resources["txt_style"];
                        brd.Child = txt;
                        brd.Style = (Style)this.Resources["border_style_for_text_2048"];
                        Ug_Map.Children.Add(brd);
                    }
                }
            }
            Check_GameOver();
        }
        private void GameWin()
        {
            WinGame_Window winGame_window = new WinGame_Window();
            winGame_window.Show();
        }

        private void GameLose()
        {
           
        }
        private void Check_GameOver()
        {
            bool Game_Win = false;
            bool Game_Lose = false;
            int Count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (map[i, j] == 2048)
                        Game_Win = true;
                    else if (map[i, j] != 0)
                        Count++;
                }
            }
            if (Count == 16 && !Game_Win)
                Game_Lose = true;
            else
                Game_Lose = false;

            if (Game_Win) GameWin();
            if (Game_Lose) GameLose();

        }
    }
} 

