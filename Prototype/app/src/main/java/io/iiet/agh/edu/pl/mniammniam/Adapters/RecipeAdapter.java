package io.iiet.agh.edu.pl.mniammniam.Adapters;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;

import io.iiet.agh.edu.pl.mniammniam.R;
import io.iiet.agh.edu.pl.mniammniam.data.Recipe;

/**
 * Created by Krzychu on 11.04.2017.
 */


public class RecipeAdapter extends RecyclerView.Adapter<RecipeAdapter.RecipeViewHolder> {
    ArrayList<Recipe> mRecipes;

    @Override
    public RecipeViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        Context context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);
        int layoutForListItem = R.layout.recipe_listview_item;
        View view = inflater.inflate(layoutForListItem, parent, false);
        RecipeViewHolder viewHolder = new RecipeViewHolder(view);
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(RecipeViewHolder holder, int position) {
        holder.onBind(mRecipes.get(position));
    }

    @Override
    public int getItemCount() {
        return mRecipes.size();
    }

    class RecipeViewHolder extends RecyclerView.ViewHolder {
        ImageView mRecipeImage;
        TextView tv_Title;
        TextView tv_Author;
        TextView tv_NumberOfPeople;

        RecipeViewHolder(View itemView) {
            super(itemView);
            mRecipeImage = (ImageView) itemView.findViewById(R.id.recipe_image);
            tv_Title = (TextView) itemView.findViewById(R.id.title);
            tv_Author = (TextView) itemView.findViewById(R.id.author);
            tv_NumberOfPeople = (TextView) itemView.findViewById(R.id.number_of_people);

        }
        public void onBind(Recipe recipe){
            tv_Title.setText(recipe.getTitle());
            tv_Author.setText(recipe.getmAuthor());
            tv_NumberOfPeople.setText(recipe.getNumberOfPeople());
            /*TODO use Picasso to add image*/
        }

    }
}
