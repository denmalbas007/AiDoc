from fastapi import FastAPI
from pydantic import BaseModel
from prediction_model_adapter import predict

app = FastAPI()


class Request(BaseModel):
    input_string: str


@app.post("/predict/")
async def render_graph(request: Request):
    return predict(request.input_string)
