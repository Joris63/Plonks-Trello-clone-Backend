const { MongoClient } = require("mongodb");

module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    switch (req.method) {
        case "GET":
            context.res = {
                status: 405,
                body: "Unfortunately you are allowed to do this"
            }
            break;
        case "POST":
            if (req?.body?.document) {
                const client = new MongoClient(process.env["MongoDBConnectionString"]);

                await client.connect();

                const db = client.db("Cluster0")

                const col = db.collection("Plonks-Docs")

                await col.insertOne(req.body.document);

                console.log(`Inserted ${req.body.document}`);

                client.close();

                context.res = {
                    body: "Inserted doc successfully",
                }

            }
            else {
                context.res = {
                    status: 400,
                    body: "You didn't insert a document"
                }
            }
            break;
    }
}