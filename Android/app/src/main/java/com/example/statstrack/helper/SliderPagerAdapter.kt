package com.example.statstrack.helper

import android.content.Context
import android.graphics.Bitmap
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import androidx.recyclerview.widget.RecyclerView
import com.example.statstrack.R

//class SliderPagerAdapter(private val context: Context, private val images: List<Int>) :
//    RecyclerView.Adapter<SliderPagerAdapter.SliderViewHolder>() {
//
//    inner class SliderViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
//        val imageView: ImageView = itemView.findViewById(R.id.imageView)
//    }
//
//    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): SliderViewHolder {
//        val view = LayoutInflater.from(context).inflate(R.layout.slider_item, parent, false)
//        return SliderViewHolder(view)
//    }
//
//    override fun onBindViewHolder(holder: SliderViewHolder, position: Int) {
//        holder.imageView.setImageResource(images[position])
//    }
//
//    override fun getItemCount(): Int = images.size
//}

class SliderPagerAdapter(private val context: Context, private val images: List<Bitmap>) :
    RecyclerView.Adapter<SliderPagerAdapter.SliderViewHolder>() {

    inner class SliderViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val imageView: ImageView = itemView.findViewById(R.id.imageView)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): SliderViewHolder {
        val view = LayoutInflater.from(context).inflate(R.layout.slider_item, parent, false)
        return SliderViewHolder(view)
    }

    override fun onBindViewHolder(holder: SliderViewHolder, position: Int) {
        holder.imageView.setImageBitmap(images[position])
    }

    override fun getItemCount(): Int = images.size
}

