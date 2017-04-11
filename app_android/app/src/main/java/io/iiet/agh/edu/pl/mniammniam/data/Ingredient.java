package io.iiet.agh.edu.pl.mniammniam.data;

/**
 * Created by Krzychu on 11.04.2017.
 */

public class Ingredient {
    int mQuantity;
    String name;
    String unit;

    public int getmQuantity() {
        return mQuantity;
    }

    public void setmQuantity(int mQuantity) {
        this.mQuantity = mQuantity;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getUnit() {
        return unit;
    }

    public void setUnit(String unit) {
        this.unit = unit;
    }

    public Ingredient(int mQuantity, String name, String unit) {

        this.mQuantity = mQuantity;
        this.name = name;
        this.unit = unit;
    }
}
