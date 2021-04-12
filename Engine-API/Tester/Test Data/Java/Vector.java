import java.util.Arrays;

public class Vector {
	
	private double[] V; 
	
	
	// Included to simplify usability & protect the private vector/array
	public double[] get() {
		return this.V;
	}
	
	// Included to simplify usability & protect the private vector/array
	public void set(double[] v) {
		this.V = v;
	}
	
	public Vector() {
        this.V = new double[0];
    }

    public Vector(int size, double D) {
		this.V = new double[size];
		for(int i=0; i<size; i++) {
			this.V[i] = D;
		}
    }

    public Vector(double[] d) {
		this.V = d;
    }

    public Vector(int[] arr) {
		this.V = new double[arr.length];
		
		for(int i=0; i<arr.length; i++) {
			this.V[i] = arr[i]; 
		}
    }

    public Vector append(double[] doubleArray) {
        double[] n = new double[this.get().length + doubleArray.length];
        
        for(int i=0;i<this.get().length;i++) {
        	n[i] = this.get()[i];
        }
        
        for(int i=this.get().length;i<n.length;i++) {
        	n[i] = doubleArray[i-this.get().length];
        }
        
        this.set(n);
        
        return this;
    }

    public Vector append(int[] intArray) {
    	double[] c = Arrays.stream(intArray).asDoubleStream().toArray();
    	double[] n = new double[this.get().length + c.length];
    	
    	for(int i=0;i<this.get().length;i++) {
        	n[i] = this.get()[i];
        }
        
        for(int i=this.get().length;i<n.length;i++) {
        	n[i] = c[i-this.get().length];
        }
        
        this.set(n);
        return this;
    }

    public Vector append(Vector V) {
    	double[] c = V.get();
    	double[] n = new double[this.get().length + c.length];
    	
    	for(int i=0;i<this.get().length;i++) {
        	n[i] = this.get()[i];
        }
        
        for(int i=this.get().length;i<n.length;i++) {
        	n[i] = c[i-this.V.length];
        }
        
        this.set(n);
        return this;
    }

    public Vector append(double aDouble) {
    	double[] n = new double[this.get().length +1];
    	
    	for(int i=0; i<n.length-1; i++) {
    		n[i] = this.V[i]; 
    	}
    	n[this.get().length] = aDouble;
    	
    	this.set(n);
        return this;
    }

    public Vector clone() {
    	return new Vector(this.get());
    }

    public Boolean equal(Vector V) {
        return Arrays.equals(V.get(), this.get());
    }

    public int getLength() {
        return this.get().length;
    }

    public Double getValue(int i) {
    	if(this.getLength()>i)
    		return this.get()[i];
    	else return null;
    }

    public Vector add(Vector v) {
    	if(this.V.length == v.V.length) {
    		for(int i=0;i<this.V.length;i++) {
    			System.out.print(this.V[i] + " + " + v.V[i]);
    			this.V[i] += v.V[i];
    			System.out.println(" = " +this.V[i]);
    		}
    		return this;
    	}
    	return null;
    }

    public Vector add(double V) {
//    	for(double d:this.get())
//    		d +=V;
    	
    	for(int i=0; i<this.get().length; i++) {
    		this.get()[i] = this.get()[i] + V;
    	}
    	
        return this;
    }

    public Vector sub(Vector v) {
    	if(this.V.length == v.V.length) {
    		for(int i=0;i<this.V.length;i++) {
    			System.out.print(this.V[i] + " - " + v.V[i]);
    			this.V[i] -= v.V[i];
    			System.out.println(" = " +this.V[i]);
    		}
    		return this;
    	}
    	return null;
    }

    public Vector subV(int l, int r) {
    	if (r < l) return null;
    	int size = r - l + 1;
    	double[] d = new double[size];
//    	double[] d = Arrays.copyOfRange(this.get(), l, r);
    	for(int i=l; i<=r; i++) {
    		d[i] = this.V[i];
    	}
    	Vector n = new Vector(d);
        return n;
    }

    public Vector Mult(Vector V) {
    	if(this.getLength() == V.getLength()) {
    		for(int i=0;i<this.getLength();i++) {
    			this.get()[i] *= V.getValue(i);
    		}
    		return this;
    	}
    	else return null;
    }

    public Vector Mult(double d) {
    	for(int i=0; i<this.V.length; i++) {
    		this.V[i] *= d;
    	}
        return this;
    }

    public Vector Normalize() {
    	double eLength = 0.0;
    	
    	for(double x:this.get())
    		eLength += (x*x);
    	
    	eLength = Math.sqrt(eLength);
    	
    	if(eLength != 0) {
	    	for(int i=0; i<this.V.length; i++) {
	    		this.V[i] /= eLength;
	    	}
    	}
    	
        return this;
    }

    public Double EuclidianDistance(Vector V) { //returns distance or -1 if vector sizes are different
    	double distance = 0.0;
    	if(this.getLength() == V.getLength()) {
    		for(int i=0;i<this.getLength();i++) {
    			double temp = this.getValue(i) - V.getValue(i);
    			temp *= temp;
    			distance += temp;
    		}
    		distance = Math.sqrt(distance);
    		return distance;
    	}
    	else return -1.0;
    }
}