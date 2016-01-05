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
using NCalc.Domain;

namespace GSA
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CrossOverProbTextBox.Text = GSA.GA.Algorithm.UniformRate.ToString();
            MutationProbTextBox.Text = GSA.GA.Algorithm.MutationRate.ToString();
            TournamentSizeTextBox.Text = GSA.GA.Algorithm.TournamentSize.ToString();
            ElitismCheckBox.IsChecked = GSA.GA.Algorithm.Elitism;

            MaxCrossOverProbTextBox.Text = GSA.GAMax.Algorithm.UniformRate.ToString();
            MaxMutationProbTextBox.Text = GSA.GAMax.Algorithm.MutationRate.ToString();
            MaxTournamentSizeTextBox.Text = GSA.GAMax.Algorithm.TournamentSize.ToString();
            MaxElitismCheckBox.IsChecked = GSA.GAMax.Algorithm.Elitism;

        }

        private void Evaluate_Function_Button_Click(object sender, RoutedEventArgs e)
        {
            try {
                LogTextBox.Document.Blocks.Clear();
                NCalc.Expression exp = new NCalc.Expression(FormulaTextBox.Text);
                if(!string.IsNullOrWhiteSpace(XValueTextBox.Text)) {
                    Double xValue = 1.0;
                    Double.TryParse(XValueTextBox.Text, out xValue);
                    exp.Parameters["x"] = xValue;
                }
                
                String result = exp.Evaluate().ToString();
                LogTextBox.AppendText(result);
            } catch(Exception ex) {
                System.Windows.MessageBox.Show(ex.ToString());
            }
          
        }

        private void GA_Simple_Button_Click(object sender, RoutedEventArgs e) {
            GSA.GA.Algorithm.UniformRate = Double.Parse(CrossOverProbTextBox.Text);
            GSA.GA.Algorithm.MutationRate = Double.Parse(MutationProbTextBox.Text);
            GSA.GA.Algorithm.TournamentSize = Int32.Parse(TournamentSizeTextBox.Text);
            GSA.GA.Algorithm.Elitism = ElitismCheckBox.IsChecked.Value;
            GSA.GA.Algorithm.Rnd = new Random();

            LogTextBox log = new GSA.LogTextBox(LogTextBox);

            GSA.GA.FitnessCalc.setSolution("1111000000000000000000000000000000000000000000000000000000001111");
            GSA.GA.Population myPop = new GSA.GA.Population(50, true);

            int generationCount = 0;
            while (myPop.getFittest().getFitness() < GSA.GA.FitnessCalc.getMaxFitness()) {
                generationCount++;
                //log.Write("Generation: " + generationCount + " Fittest: " + myPop.getFittest().getFitness(), Brushes.Yellow);
                myPop = GSA.GA.Algorithm.evolvePopulation(myPop);
            }
            log.Write("Solution found!", Brushes.GreenYellow);
            log.Write("Generation: " + generationCount);
            log.Write("Genes:");
            log.Write(myPop.getFittest().ToString());
        }

        private void MaxButton_Click(object sender, RoutedEventArgs e) {

            performAlgorithm(0);

        }

        private void MinButton_Click(object sender, RoutedEventArgs e) {
            performAlgorithm(1);
        }

        private void performAlgorithm(int maxOrMin) {
            GSA.GAMax.Algorithm.UniformRate = Double.Parse(MaxCrossOverProbTextBox.Text);
            GSA.GAMax.Algorithm.MutationRate = Double.Parse(MaxMutationProbTextBox.Text);
            GSA.GAMax.Algorithm.TournamentSize = Int32.Parse(MaxTournamentSizeTextBox.Text);
            GSA.GAMax.Algorithm.Elitism = MaxElitismCheckBox.IsChecked.Value;
            GSA.GAMax.Algorithm.DieFactor = DieFactorCheckBox.IsChecked.Value;
            GSA.GAMax.Algorithm.Rnd = new Random();
            GSA.GAMax.Individual.Rnd = new Random();
            GSA.GAMax.Algorithm.bestSolution = null;

            LogTextBox log = new GSA.LogTextBox(LogTextBox);

            log.Write("Constants setted", Brushes.BlueViolet);
            GSA.GAMax.Individual.setDefaultGeneLength(int.Parse(BinaryLengthTextBox.Text));
            GSA.GAMax.Population maxPop = new GSA.GAMax.Population(int.Parse(PopulationNumberTextBox.Text), true);
            GSA.GAMax.FitnessCalc._op = (GSA.GAMax.Operation)maxOrMin;
            if (GSA.GAMax.FitnessCalc.setFunctionToOperate(FormulaTextBox.Text)) {
                log.Write(String.Format("Function setted: {0}", GSA.GAMax.FitnessCalc._exp.ParsedExpression.ToString()), Brushes.Azure);
                //setear población
                log.Write("Initial population, size: " + maxPop.size().ToString(), Brushes.Turquoise);
                for (int i = 0; i < maxPop.size(); i++) {
                    log.Write(maxPop.getIndividual(i).ToString(), Brushes.Wheat);
                }
            } else {
                log.Write(String.Format("Your function has errors: {0}", GSA.GAMax.FitnessCalc._exp.Error), Brushes.PaleVioletRed);
                log.Write("The function must be correct and contains an 'x' between []. Example 'Pow([x], 3) + 5*[x] - 2 / ([x]+2)", Brushes.PaleVioletRed);
            }

            int generationCount = 0;
            int MaxGeneration = int.Parse(MaxIterationsTextBox.Text);
            while (generationCount <= MaxGeneration) {
                generationCount++;
                //log.Write("Generation: " + generationCount + " Fittest: " + myPop.getFittest().getFitness(), Brushes.Yellow);
                GSA.GAMax.FitnessCalc.CurrentPopulation = maxPop;
                maxPop = GSA.GAMax.Algorithm.evolvePopulation(maxPop);

            }
            log.Write("Solution found!", Brushes.GreenYellow);
            log.Write("Generation: " + generationCount);
            log.Write("Genes:");
            GSA.GAMax.Individual bestSolutionFound = GSA.GAMax.FitnessCalc.getFittest();
            if (!GSA.GAMax.Algorithm.betterFitness(GSA.GAMax.Algorithm.bestSolution, bestSolutionFound)) {
                bestSolutionFound = GSA.GAMax.Algorithm.bestSolution;
            }
            log.Write(bestSolutionFound.ToString());

        }

        private void BinaryLengthTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            String valueFormat = "Min: {0} / Max: {1}";
            int binaryLength = 0;
            if (int.TryParse(BinaryLengthTextBox.Text, out binaryLength)) {
                valuesRangeTExtBlock.Text = string.Format(valueFormat, "0", Math.Pow(2, binaryLength).ToString());
            }
        }

        private void ClearTextBox_MenuItem_Click(object sender, RoutedEventArgs e) {
            LogTextBox.Document.Blocks.Clear();
        }

       
    }
}
