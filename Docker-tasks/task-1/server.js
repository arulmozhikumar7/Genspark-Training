const express = require("express");
const app = express();

app.use('',(req,res)=>{
     res.status(200).send(`<html>
       <p>Hello World</p> 
        </html>`)
})

app.listen(3000,()=>{
   console.log("Server Running")
})