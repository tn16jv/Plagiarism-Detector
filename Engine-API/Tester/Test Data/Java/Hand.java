

import java.util.LinkedList;


public interface Hand {
    
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
    
    
}
