const express = require('express');
const cors = require('cors');
const app = express();

app.use(cors());
const PORT = 3000;

app.get('/', (req, res) => {
    res.json({ message: 'Hello from backend!' });
});

app.listen(PORT, () => {
    console.log(`Backend running on port ${PORT}`);
});
