package io.iiet.agh.edu.pl.mniammniam;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;

public class AddRecipeActivity extends AppCompatActivity {

    private TextView mTitle;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_recipe);
        mTitle= (TextView) findViewById(R.id.tv_recipe_title);
    }
}
