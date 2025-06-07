/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Windows;
using System.Windows.Controls;
using Forgotten_Land_Launcher.Library;

namespace Forgotten_Land_Launcher.Modules.Characters_Market
{
    /// <summary>
    /// Interaction logic for Profession_Span.xaml
    /// </summary>
    public partial class Profession_Span : UserControl
    {
        int pProfSkill;
        int pProfMinSkill;
        int pProfMaxSkill;

        public Profession_Span(int _profSkill, int _profMinSkill, int _profMaxSkill)
        {
            InitializeComponent();

            pProfSkill = _profSkill;
            pProfMinSkill = _profMinSkill;
            pProfMaxSkill = _profMaxSkill;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ProfBar.Minimum = pProfMinSkill;
            ProfBar.Maximum = pProfMaxSkill;
            ProfBar.Value = pProfMinSkill;

            Extensions.SetImageBrushSource(ProfImage, WoW_Definitions.GetProfessionIcon(pProfSkill), UriKind.Relative);

            ProfName.Text = WoW_Definitions.GetProfessionName(pProfSkill);
            ProfMinSkill.Text = pProfMinSkill.ToString();
            ProfMaxSkill.Text = pProfMaxSkill.ToString();
        }
    }
}
