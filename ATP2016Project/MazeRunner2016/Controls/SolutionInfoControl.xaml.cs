using MazeLib;
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

namespace MazeRunner2016.Controls
{
    /// <summary>
    /// Interaction logic for SolutionInfoControl.xaml
    /// </summary>
    public partial class SolutionInfoControl : UserControl
    {
        private Solution mySolution;
        private string statesDeveloped, timeToSolve;

        public SolutionInfoControl()
        {
            InitializeComponent();
        }

        public SolutionInfoControl(Solution solution, string timeToSolve, string statesDeveloped)
        {
            InitializeComponent();
            this.statesDeveloped = statesDeveloped;
            this.timeToSolve = timeToSolve;
            mySolution = solution;
            updateTexts();
        }

        private void updateTexts()
        {
            numOfStepsTxt.Text = mySolution.getSolutionNumberOfSteps().ToString();
            List<AState> solPath = mySolution.getSolutionPath();
            List<string> solPathPositions = new List<string>();
            foreach (AState state in solPath)
            {
                string position = (state as MazeState).Position.ToString();
                solPathPositions.Add(position);
            }
            listOfStepsLst.ItemsSource = solPathPositions;
            numOfStatsTxt.Text = statesDeveloped;
            timeTxt.Text = timeToSolve;
        }
    }
}
