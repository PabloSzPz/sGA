using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA.GA
{
    public class Individual
    {
        static int defaultGeneLength = 64;
        private byte[] genes = new byte[defaultGeneLength];
        //Cache
        private int fitness = 0;

        //Create a random individual
        public void generateIndividual()
        {
            Random rnd = new Random();
            for (int i=0; i<size();i++)
            {
                byte gene = (byte)Math.Round(rnd.NextDouble());
                genes[i] = gene;
            }

        }

        #region Public Methods

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
        public int getFitness()
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
            int value = BitConverter.ToInt32(genes, 0); 
            return geneString;
        }


    }
}
