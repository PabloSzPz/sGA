using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA.GA {
    public class FitnessCalc {

        static byte[] solution = new byte[64];

        /* Public methods */
        // Set a candidate solution as a byte array
        public static void setSolution(byte[] newSolution) {
            solution = newSolution;
        }

        // To make it easier we can use this method to set our candidate solution 
        // with string of 0s and 1s
        public static void setSolution(String newSolution) {
            solution = new byte[newSolution.Length];
            // Loop through each character of our string and save it in our byte 
            // array
            for (int i = 0; i < newSolution.Length; i++) {
                String character = newSolution.Substring(i, 1);
                if (character.Contains("0") || character.Contains("1")) {
                    solution[i] = Convert.ToByte(character);
                } else {
                    solution[i] = 0;
                }
            }
        }

        // Calculate inidividuals fittness by comparing it to our candidate solution
        public static int getFitness(Individual individual) {
            int fitness = 0;
            // Loop through our individuals genes and compare them to our cadidates
            for (int i = 0; i < individual.size() && i < solution.Length; i++) {
                if (individual.getGene(i) == solution[i]) {
                    fitness++;
                }
            }
            return fitness;
        }

        // Get optimum fitness
        public static int getMaxFitness() {
            int maxFitness = solution.Length;
            return maxFitness;
        }
    }

}
