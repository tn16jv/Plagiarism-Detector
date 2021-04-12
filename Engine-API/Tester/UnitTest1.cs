using NUnit.Framework;
using Plagiarism_Engine.Controllers;
using Plagiarism_Engine.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Plagiarism_Engine_Models;

namespace Tests
{
    public class Tests
    {
        HttpClient client;
        string baseURL = "http://localhost:5668/api/";
        [SetUp]
        public void Setup()
        {
            client = new HttpClient();
        }

        [Test]
        public async Task Test1()
        {

            File one = new File
            {
                Id = 1,
                FileType = "Java",
                isExempt = false,
                CodeFiles = @"public class Hand {
    
    /**
     * @return LinkedList<PlayingCard> the hand
     */
    LinkedList<PlayingCard> getHand();

    /**
     * @return int the number of cards in the hand
     */
    int getNumberOfCards();
    
    /**
     * @param i index of card to return
     * @return return card at index i (0 based)
     */
    PlayingCard getCard(int i);
    
    /**
     * @param i index of card to return
     * @return returns card removed from hand
     */
    PlayingCard removeCard(int i);
    
    /**
     * @param p adds PlayingCard to hand
     */
    void addCard(PlayingCard p);
    
    /**
     * @return the tally for the hand
     */
    default int getTally(){
        int temp=0;
        LinkedList<PlayingCard> hand = getHand();
        for(PlayingCard c: hand)
            temp += c.rank;
        return temp;
    }
    
    @Override
    String toString();
    
    
}"

            };

            File two = new File
            {
                Id = 2,
                FileType = "Java",
                isExempt = false,
                CodeFiles = @"public class Hand {
    
    /**
     * @return LinkedList<PlayingCard> the hand
     */
    LinkedList<PlayingCard> getHand();

    /**
     * @return int the number of cards in the hand
     */
    int getNumberOfCards();
    
    /**
     * @param i index of card to return
     * @return return card at index i (0 based)
     */
    PlayingCard getCard(int i);
    
    /**
     * @param i index of card to return
     * @return returns card removed from hand
     */
    PlayingCard removeCard(int i);
    
    /**
     * @param p adds PlayingCard to hand
     */
    void addCard(PlayingCard p);
    
    /**
     * @return the tally for the hand
     */
    default int getTally(){
        int temp=0;
        LinkedList<PlayingCard> hand = getHand();
        for(PlayingCard c: hand)
            temp += c.rank;
        return temp;
    }
    
    @Override
    String toString();
    
    
}"
            };
            

            List<File> f = new List<File>();
            f.Add(one);
            f.Add(two);
            //f.Add(three);

            var t = Newtonsoft.Json.JsonConvert.SerializeObject(f);

            var controller = new CompareController();
            //List<Report> r = controller.postCompare(f, "0e3a3316-8fc7-4a8b-a6a8-84cd8dc56c17");

            //var o = Newtonsoft.Json.JsonConvert.SerializeObject(r);


             HttpResponseMessage response = await client.GetAsync(string.Format("{0}{1}", baseURL, "Compare"));
            
        }
    }
}