using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA.GAMax {
    public class Algorithm {
        //GA parameters
        private static double uniformRate = 0.5;
        private static double mutationRate = 0.015;
        private static int tournamentSize = 5;
        private static bool elitism = true;
        private static bool dieFactor = true;
        private static int numberElitism = 1;

        public static Individual bestSolution = null;

        private static Random rnd = new Random();

        #region Propiedades
        public static double UniformRate {
            get {
                return uniformRate;
            }

            set {
                uniformRate = value;
            }
        }

        public static double MutationRate {
            get {
                return mutationRate;
            }

            set {
                mutationRate = value;
            }
        }

        public static int TournamentSize {
            get {
                return tournamentSize;
            }

            set {
                tournamentSize = value;
            }
        }

        public static bool Elitism {
            get {
                return elitism;
            }

            set {
                elitism = value;
            }
        }

        public static int NumberElitism {
            get {
                return numberElitism;
            }

            set {
                numberElitism = value;
            }
        }

        public static Random Rnd {
            get {
                return rnd;
            }

            set {
                rnd = value;
            }
        }

        public static bool DieFactor {
            get {
                return dieFactor;
            }

            set {
                dieFactor = value;
            }
        }
        #endregion

        public static bool betterFitness(Individual bestSol, Individual newBestSol) {
            if (bestSol.getFitness() != double.Epsilon) {
                if (FitnessCalc._op == Operation.Max) {
                    return bestSol.getFitness() <= newBestSol.getFitness();
                } else {
                    return bestSol.getFitness() >= newBestSol.getFitness();
                }
            } else { return false; }
        }

        //Evolve population
        public static Population evolvePopulation(Population pop) {
            Population newPopulation = new Population(pop.size(), false);
            Individual fittestOfThisPop = null;
            //keep your best individual
            if (Elitism) {
                //get fittest of the CURRENT pop
                fittestOfThisPop = newPopulation.saveIndividual(0, FitnessCalc.getFittest());

                //SAVE best of the algorithm
                if(bestSolution != null) {
                    if (betterFitness(bestSolution, fittestOfThisPop)) {
                        bestSolution = fittestOfThisPop;
                    }
                } else {
                    bestSolution = fittestOfThisPop;
                }
            }

            //Crossover population
            int elitismOffset;
            if (Elitism) {
                elitismOffset = 1;
            } else {
                elitismOffset = 0;
            }

            //loop over tje population size and create new individuals with crossover
            for(int i = elitismOffset; i<pop.size();i++) {
                Individual indiv1 = rouletteSelection(pop);
                Individual indiv2 = rouletteSelection(pop);
                Individual newIndiv = crossover(indiv1, indiv2);
                newPopulation.saveIndividual(i, newIndiv);
            }

            //Mutate population
            for (int i=elitismOffset; i<newPopulation.size();i++) {
                mutate(newPopulation.getIndividual(i));
            }

            // fitless die & replace by fittest
            if (DieFactor) {
                if (elitism) {
                    newPopulation.saveIndividual(FitnessCalc.getFitlessIndex(), fittestOfThisPop);
                } else {
                    newPopulation.saveIndividual(FitnessCalc.getFitlessIndex(), FitnessCalc.getFittest());
                }
            }




            return newPopulation;
        }

        // Crossover individuals
        private static Individual crossover(Individual indiv1, Individual indiv2) {
            Individual newSol = new Individual();
            // Loop through genes
            for (int i = 0; i < indiv1.size(); i++) {
                // Crossover
                if (Rnd.NextDouble() <= UniformRate) {
                    newSol.setGene(i, indiv1.getGene(i));
                } else {
                    newSol.setGene(i, indiv2.getGene(i));
                }
            }
            return newSol;
        }

        // Mutate an individual
        private static void mutate(Individual indiv) {
            // Loop through genes
            for (int i = 0; i < indiv.size(); i++) {
                if (Rnd.NextDouble() <= MutationRate) {
                    // Create random gene
                    byte gene = (byte)Math.Round(Rnd.NextDouble());
                    indiv.setGene(i, gene);
                }
            }
        }

        // Select individuals for crossover
        private static Individual rouletteSelection(Population pop) {
            double[] weights = new double[pop.size()];
            for(int i =0; i < pop.size(); i++) {
                weights[i] = FitnessCalc.getProb(pop.getIndividual(i));
            }

            double weight_sum = 0;
            for (int i = 0; i < weights.Length; i++) {
                weight_sum += weights[i];
            }
            // get a random value
            double value = randUniformPositive() * weight_sum;
            // locate the random value based on the weights
            for (int i = 0; i < weights.Length; i++) {
                value -= weights[i];
                if (value <= 0) return pop.getIndividual(i);
            }
            // only when rounding errors occur
            return pop.getIndividual(weights.Length - 1);

        }

        // Returns a uniformly distributed double value between 0.0 and 1.0
        static double randUniformPositive() {
            // easiest implementation
            return new Random().NextDouble();
        }

    }
}
