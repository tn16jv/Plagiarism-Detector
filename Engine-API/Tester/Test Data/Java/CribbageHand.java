
import java.util.LinkedList;


public class CribbageHand implements Hand{
    
    private PlayingCard Tu;
    private Boolean crib;
    
    private LinkedList<PlayingCard> hard = new LinkedList<PlayingCard>();
    
    /**
     * @param turnup 	the turnup card
     * @param isCrib	if the hand is a crib
     * @param l			The initial cards for this hand
     */
    public CribbageHand(PlayingCard turnup, Boolean isCrib,LinkedList<PlayingCard> l){
        Tu = turnup;
        crib = isCrib;
        for(PlayingCard p:l)
        	hard.add(p);
    }
    
    /**
     * @return true if crib
     */
    public Boolean isCrib(){
        return crib;
    }

   
    @Override
    public int getNumberOfCards() {
       return hard.size();
    }

    @Override
    public PlayingCard getCard(int i) {
        return hard.get(i);
    }

    @Override
    public PlayingCard removeCard(int i) {
        return hard.remove(i);
    }

    
    @Override
    public void addCard(PlayingCard p) {//check to see if 4 cards are already in hand
        hard.add(p);
    }
    
    
    @Override
    public int getTally(){
        int tally = 0;
      //flush
        if(!crib)
	        if(hard.get(0).suit.equals(hard.get(1).suit) && hard.get(1).suit.equals(hard.get(2).suit) && hard.get(2).suit.equals(hard.get(3).suit)){
	            tally = 4;
	            if(hard.get(0).suit.equals(Tu.suit)) tally = 5;
	        }
        else//if crib
        	if(hard.get(0).suit.equals(hard.get(1).suit) && hard.get(1).suit.equals(hard.get(2).suit) && hard.get(2).suit.equals(hard.get(3).suit) && hard.get(0).suit.equals(Tu.suit)) tally = 5;
      //any pairs
        for(int i=0;i<4;i++){
            int val = hard.get(i).rank;
            for(int j=i+1;j<4;j++){
                int comp = hard.get(j).rank;
                if(val == comp) tally+=2;
            }
            if(val == Tu.rank) tally+=2;
        }
        
      //combos of 2
        for(int i=0;i<4;i++){//combos of 2
        	if(hard.get(i).value + Tu.value == 15) tally +=2;
            for(int j=i;j<4;j++){
            	if(i != j)
                if(j<4)
                    if(hard.get(i).value + hard.get(j).value == 15) tally +=2;    
            }
        }
      //combos of 3
        for(int i=0;i<5-2;i++){//combos of 3
            for(int j=i+1;j<5-1;j++){
            	if(hard.get(i).value + hard.get(j).value + Tu.value == 15) tally +=2;
                for(int k=j+1;k<5;k++){
                    if(k<4)
                        if(hard.get(i).value + hard.get(j).value +hard.get(k).value == 15) tally +=2;   
                }
            }
        }
      //combos of 4
        for(int i=0;i<5-3;i++){
            for(int j=i+1;j<5-2;j++){
                for(int k=j+1;k<5-1;k++){
                	if(hard.get(i).value + hard.get(j).value + hard.get(k).value + Tu.value == 15) tally +=2;
                    for (int l=k+1;l<5;l++){
                        if(l<4)
                            if(hard.get(i).value + hard.get(j).value + hard.get(k).value + hard.get(l).value == 15) tally +=2;  
                    }
                }
            }
        }
      //combos of 5
        if(hard.get(0).value + hard.get(1).value + hard.get(2).value + hard.get(3).value + Tu.value == 15)
            tally +=2;
        
        
    //runs
        LinkedList<PlayingCard> temp = (LinkedList)hard.clone();
        temp.add(Tu);
        temp.sort(new CardCompare());

        int runNum = 1;
        int multiple = 1;
        PlayingCard last = temp.get(0);
        
        for(int i=1;i<5;i++) {
        	if(last.rank+1 == temp.get(i).rank) 
        		runNum++;
        	else if(last.rank == temp.get(i).rank)
        		multiple++;
        	else {
        		runNum = 1;
        		multiple = 1;
        	}
        		
        	last = temp.get(i);
        }
        if(runNum>2)
        	tally += (runNum*multiple);
        
    //Nob
        for(PlayingCard p:hard) {
        	if(p.rank == 11 && p.suit.equals(Tu.suit))
        		tally++;
        }
        
        return tally;
    }
    
    @Override
    public String toString() {
    	String temp = "";
        for(PlayingCard p:hard) {
        	temp = temp + p.toString();
        }
        return temp;
    }

	@Override
	public LinkedList<PlayingCard> getHand() {
		return hard;
	}	
    
}
