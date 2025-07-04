const express = require('express');
const { MongoClient } = require('mongodb');

const app = express();
const PORT = 3000;
const mongoUrl = process.env.MONGO_URL;

app.get('/', async (req, res) => {
  try {
    const client = await MongoClient.connect(mongoUrl);
    const db = client.db(); 
    const collections = await db.listCollections().toArray();
    await client.close();

    res.json({ message: 'Connected to MongoDB!', collections });
  } catch (err) {
    res.status(500).json({ error: 'Failed to connect to MongoDB', details: err.message });
  }
});

app.listen(PORT, () => {
  console.log(`Server running at Port : ${PORT}`);
});
