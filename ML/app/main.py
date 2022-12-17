from fastapi import FastAPI
from pydantic import BaseModel
from prediction_model_adapter import Adapter

app = FastAPI()
addapter = Adapter()

class Request(BaseModel):
    input_string: str


@app.post("/predict/")
async def render_graph(request: Request):
    sentences = addapter.predict_sentences(request.input_string)
    sentences_new = {}
    counter = 0
    for k in sentences.keys():
        sentences_new[k] = sentences[k]
        counter += 1
        if counter > 3:
            break
    return {"prediction": addapter.predict(request.input_string),
            "prediction_sentences": list(sentences_new.keys())}
