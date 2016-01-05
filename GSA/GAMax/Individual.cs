using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA.GAMax {
    
    public class Individual
    {
        private static Random rnd = new Random();
        static int defaultGeneLength = 5;
        private byte[] genes = new byte[defaultGeneLength];
        //Cache
        private double fitness = 0;

        //Create a random individual
        public void generateIndividual()
        {
           
            for (int i=0; i<size();i++)
            {
                byte gene = (byte)Math.Round(rnd.NextDouble());
                genes[i] = gene;
            }

        }

        #region Public Methods

        public static Random Rnd {
            get {
                return rnd;
            }

            set {
                rnd = value;
            }
        }

        //Change GeneLenght
        public static void setDefaultGeneLength(int length)
        {
            defaultGeneLength = length;
        }

        //Get gene by position
        public byte getGene(int index)
        {
            return genes[index];
        }

        //Change gene at position
        public void setGene(int index, byte value)
        {
            genes[index] = value;
            fitness = 0;
        }

        //get size
        public int size()
        {
            return genes.Length;
        }

        //getFitness
        public double getFitness()
        {
            if (fitness == 0)
            {
                fitness = FitnessCalc.getFitness(this);
            }
            return fitness;
        }

        #endregion

        public override string ToString()
        {
            String geneString = "";
            for (int i = 0; i < size(); i++)
            {
                geneString += getGene(i);
            }
            int value = Convert.ToInt32(geneString, 2); 
            return geneString + " : " + value.ToString();
        }

        public int numericValue() {
            String geneString = "";
            for (int i = 0; i < size(); i++) {
                geneString += getGene(i);
            }
            return Convert.ToInt32(geneString, 2);
        }


    }
}
