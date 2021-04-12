
import java.util.Collections;
import java.util.LinkedList;
import java.util.List;


public class Deck {
    
    private LinkedList<PlayingCard> deck;
    
    /**
     *  constructor
     */
    public Deck(){
        deck = new LinkedList<PlayingCard>();
        String curSuit = null;
        for(int i=0;i<4;i++){
            if(i==0)curSuit = "H";
            if(i==1)curSuit = "D";
            if(i==2)curSuit = "C";
            if(i==3)curSuit = "S";
            
            deck.add(new Ace(curSuit));
            
            for(int j=2;j<14;j++){
                if(j<=10)deck.add(new NumberCard(curSuit,j));
                if(j>10)deck.add(new FaceCard(curSuit,j));
            }
        }
    }
    
    /**
     * @return one playing card from deck
     */
    public PlayingCard deal(){
        return draw(1).get(0);
    }
    
    /**
     * @param amount to remove from the deck 
     * @return amount number of cards off the top of the deck
     */
    public LinkedList<PlayingCard> draw(int amount){
    	LinkedList<PlayingCard> temp = new LinkedList<PlayingCard>();
    	for(int i=0;i<amount;i++) 
    		temp.add(deck.remove(i));
    		
        return temp;
    }
    
    /**
     * @return the number of cards remaining in the deck
     */
    public int getNumberOfCards(){
        return deck.size();
    }
    
    /**
     * Randomizes the order of the deck
     */
    public void shuffle(){
        Collections.shuffle(deck);
    }
    
    @Override
    public String toString() {
        String temp = "";
        for(PlayingCard p:deck) {
        	temp = temp + p.toString();
        }
        return temp;
    }
    
}
