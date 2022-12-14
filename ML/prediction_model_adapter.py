import pandas as pd
from datasets import load_dataset
import preprocess
import torch
import json
from transformers import RobertaConfig, RobertaModelWithHeads, EvalPrediction, TrainingArguments, AdapterTrainer, \
    RobertaTokenizer, TextClassificationPipeline
from tqdm.notebook import tqdm

training_args = TrainingArguments(
    learning_rate=1e-4,
    num_train_epochs=13,
    per_device_train_batch_size=16,
    per_device_eval_batch_size=16,
    logging_steps=10,
    output_dir="./training_output",
    overwrite_output_dir=False,
    # The next line is important to ensure the dataset labels are properly passed to the model
    remove_unused_columns=True,

)


def load_labels():
    with open("labels.json", "r") as labels:
        labels = json.load(labels)
        return labels


class Adapter:
    def __init__(self, text: str):
        self.text = text

    def load_config(self):
        config = RobertaConfig.from_pretrained(
            'ML/models/adapter/config.json',
            num_labels=2,
        )
        return config

    def load_model(self):
        model = RobertaModelWithHeads.from_pretrained(
            'ML/models/adapter/pytorch_model.bin',
            config=Adapter.load_config()
        )
        return model

    def load_tokenizer(self):
        tokenizer = RobertaTokenizer.from_pretrained('sberbank-ai/ruRoberta-large')
        return tokenizer

    def classify(self):
        classifier = TextClassificationPipeline(model=Adapter.load_model(), tokenizer=Adapter.load_tokenizer(),
                                                device=training_args.device.index)
        return classifier(self.text)

def predict(text: str) -> str:
    adapter = Adapter(text)
    prediction = adapter.classify()
    return prediction[0]['label']

