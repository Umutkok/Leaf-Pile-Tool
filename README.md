# Leaf Pile
Inspired by the leaf piles in Death’s Door, I created a small interactive leaf pile system. When the player interacts with the pile, leaves scatter and fall around the environment.

To keep performance efficient, I used object pooling for the falling and scattered leaves so that objects are reused instead of constantly instantiated and destroyed. The leaves inside the pile are also combined into a single texture atlas, which helps reduce the number of draw calls and improves batching.

Additionally, I used Unity’s Particle System to add smaller flying leaves and subtle motion, making the pile feel more dynamic and alive.


### Here is final results
<p align="center">
  <img src="https://github.com/user-attachments/assets/d14e2acf-6512-4500-81de-7e61a61abee5" alt="GIF" />
</p>

### Performance

<p align="center">
  <img src="https://github.com/user-attachments/assets/099487be-0647-491e-8c88-bd416312996f" alt="GIF" /><br />
  <small>Pile</small>
</p>

### Inspiration

<p align="center">
  <img src="https://github.com/user-attachments/assets/cf92945f-43dd-4dbf-930d-e83785de0cde" alt="GIF" /><br />
  <small>pPile</small>
</p>
