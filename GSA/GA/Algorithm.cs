using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA.GA {
    class Algorithm {
        //GA parameters
        private static double uniformRate = 0.5;
        private static double mutationRate = 0.015;
        private static int tournamentSize = 5;
        private static bool elitism = true;
        private static int numberElitism = 1;

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
        #endregion

        //Evolve population
        public static Population evolvePopulation(Population pop) {
            Population newPopulation = new Population(pop.size(), false);

            //keep your best individual
            if (Elitism) {
                newPopulation.saveIndividual(0, pop.getFittest());
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
                Individual indiv1 = tournamentSelection(pop);
                Individual indiv2 = tournamentSelection(pop);
                Individual newIndiv = crossover(indiv1, indiv2);
                newPopulation.saveIndividual(i, newIndiv);

            }

            //Mutate population
            for (int i=elitismOffset; i<newPopulation.size();i++) {
                mutate(newPopulation.getIndividual(i));
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
        private static Individual tournamentSelection(Population pop) {
            // Create a tournament population
            Population tournament = new Population(TournamentSize, false);
            // For each place in the tournament get a random individual
            for (int i = 0; i < TournamentSize; i++) {
                int randomId = (int)(Rnd.NextDouble() * pop.size());
                tournament.saveIndividual(i, pop.getIndividual(randomId));
            }
            // Get the fittest
            Individual fittest = tournament.getFittest();
            return fittest;
        }

    }
}
