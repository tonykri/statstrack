package com.example.statstrack.helper.apiCalls.dto.request

import java.util.UUID

data class ReviewCreateRequest(
    val businessId: UUID,
    val stars: Int,
    val content: String
    ) {
        init {
            require(stars in 1..5) { "Stars must be between 1 and 5." }
            require(content.length in 1..200) { "Content length must be between 1 and 200 characters." }
        }
}
