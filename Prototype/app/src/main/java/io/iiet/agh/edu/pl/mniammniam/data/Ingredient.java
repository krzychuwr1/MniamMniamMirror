package io.iiet.agh.edu.pl.mniammniam.data;

/**
 * Created by Krzychu on 11.04.2017.
 */

class Ingredient {
    private int mQuantity;
    private String name;
    private String unit;

    public int getQuantity() {
        return mQuantity;
    }

    public void setQuantity(int mQuantity) {
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
