using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA.GAMax {
    public class FitnessCalc {

        public static Operation _op = Operation.Max;
        public static NCalc.Expression _exp;
        private static Population currentPopulation;
        private static double _sumPopulationFitness;
        private static double _avgPopulationFitness;


        //Set the function to maximize/minimize
        public static bool setFunctionToOperate(string functionAsText) {
            bool canEvaluate = false;
            _exp = new NCalc.Expression(functionAsText);
            NCalc.Expression _expTest = new NCalc.Expression(functionAsText);
            _expTest.Parameters["x"] = 1;
            try {
                _expTest.Evaluate();
                canEvaluate = true;
            } catch {
                canEvaluate = false;
            }
           
            return !_exp.HasErrors() && !_expTest.HasErrors() && canEvaluate;
        }

        // Calculate inidividuals fittness by comparing it to our candidate solution
        public static double getFitness(Individual individual) {
            //fitness is just calculate de function
            //------------------------------------
            _exp.Parameters["x"] = individual.numericValue();
            double result = double.Parse(_exp.Evaluate().ToString());
            if(double.IsPositiveInfinity(result) || double.IsNegativeInfinity(result) || double.IsNaN(result)) {
                return double.Epsilon;
            } else {
                return result;
            }
            
        }

        public static Individual getFittest() {
            Individual fittest = currentPopulation.getIndividual(0);
            for (int i = 0; i < currentPopulation.size(); i++) {
                if (betterFitness(fittest, i)) {
                    fittest = currentPopulation.getIndividual(i);
                }
            }
            return fittest;
        }

        public static int getFitlessIndex() {
            Individual fitless = currentPopulation.getIndividual(0);
            int index = -1;
            for (int i = 0; i < currentPopulation.size(); i++) {
                if (!betterFitness(fitless, i)) {
                    fitless = currentPopulation.getIndividual(i);
                    index = i;
                }
            }
            return index;
        }


        private static bool betterFitness(Individual fittest, int i) {
            if (fittest.getFitness() != double.Epsilon) {
                if (_op == Operation.Max) {
                    return fittest.getFitness() <= currentPopulation.getIndividual(i).getFitness();
                } else {
                    return fittest.getFitness() >= currentPopulation.getIndividual(i).getFitness();
                }
            } else { return false; }
        }

        public static double getProb(Individual individual) {
            if(individual.getFitness() != double.Epsilon) {
                double prob = individual.getFitness() / _sumPopulationFitness;
                if (_op == Operation.Max) {
                    return prob;
                } else {
                    return (1 - prob);
                }
            } else {
                return 0.0;
            }
           
        }



        //setter
        public static Population CurrentPopulation {
            get {
                return currentPopulation;
            }

            set {
                currentPopulation = value;
                recalculateSumAndAvgFitness();
            }
        }

        private static void recalculateSumAndAvgFitness() {
            double sumPopulationFitness = 0.0;
            for (int i = 0; currentPopulation.size() < i; i++) {
                sumPopulationFitness += currentPopulation.getIndividual(i).getFitness();
            }
            _sumPopulationFitness = sumPopulationFitness;
            _avgPopulationFitness = sumPopulationFitness / currentPopulation.size();
        }

        public static double getExpectedCount(Individual individual) {
            double expectedCount = individual.getFitness() / _avgPopulationFitness;
            return expectedCount;
        }

        public static double getActualCount(Individual individual) {
            return Math.Round(getExpectedCount(individual));
        }
    }

    public enum Operation {
        Max = 0,
        Min = 1
    }

}
