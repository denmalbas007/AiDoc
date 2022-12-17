from fastapi import FastAPI
from pydantic import BaseModel
from prediction_model_adapter import predict, load_model, load_config

app = FastAPI()


class Request(BaseModel):
    input_string: str


@app.post("/predict/")
async def render_graph(request: Request):
    return {"prediction": predict(request.input_string)}
