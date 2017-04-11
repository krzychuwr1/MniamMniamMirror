package io.iiet.agh.edu.pl.mniammniam.data;

import android.util.Pair;

import java.net.URI;
import java.util.ArrayList;
import java.util.Map;
import java.util.Set;

/**
 * Created by Krzychu on 11.04.2017.
 */

public class Recipe {
    private ArrayList<Ingredient> mIngredients;
    private Set<String> mTags;
    private String mTitle;
    private String mAuthor;
    private URI mImageUri;

    public String getmAuthor() {
        return mAuthor;
    }

    public void setmAuthor(String mAuthor) {
        this.mAuthor = mAuthor;
    }

    public URI getmImageUri() {
        return mImageUri;
    }

    public void setmImageUri(URI mImageUri) {
        this.mImageUri = mImageUri;
    }

    private int mNumberOfPeople;
    private int mStarScore;
    private ArrayList<String> mSteps;

    public Recipe(ArrayList<Ingredient> mIngredients, Set<String> mTags, String mTitle, int mNumberOfPeople, int mStarScore, ArrayList<String> mSteps) {
        this.mIngredients = mIngredients;
        this.mTags = mTags;
        this.mTitle = mTitle;
        this.mNumberOfPeople = mNumberOfPeople;
        this.mStarScore = mStarScore;
        this.mSteps = mSteps;
    }

    public ArrayList<Ingredient> getIngredience() {
        return mIngredients;
    }

    public void setIngredients(ArrayList<Ingredient> mIngredients) {
        this.mIngredients = mIngredients;
    }

    public Set<String> getTags() {
        return mTags;
    }

    public void setTags(Set<String> mTags) {
        this.mTags = mTags;
    }

    public String getTitle() {
        return mTitle;
    }

    public void setTitle(String mTitle) {
        this.mTitle = mTitle;
    }

    public int getNumberOfPeople() {
        return mNumberOfPeople;
    }

    public void setNumberOfPeople(int mNumberOfPeople) {
        this.mNumberOfPeople = mNumberOfPeople;
    }

    public int getStarScore() {
        return mStarScore;
    }

    public void setStarScore(int mStarScore) {
        this.mStarScore = mStarScore;
    }

    public ArrayList<String> getSteps() {
        return mSteps;
    }

    public void setSteps(ArrayList<String> mSteps) {
        this.mSteps = mSteps;
    }

    public void addStep(String step) {
        mSteps.add(step);
    }

    public void addTag(String tag) {
        mSteps.add(tag);
    }

    public void addIngredient(Ingredient ingredient) {
        mIngredients.add(ingredient);
    }

}
