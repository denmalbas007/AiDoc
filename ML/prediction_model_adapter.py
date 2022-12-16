import json
from transformers import RobertaConfig, RobertaModelWithHeads, TrainingArguments, \
    RobertaTokenizer, TextClassificationPipeline
import torch
from preprocess import preprocess

device = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")

training_args = TrainingArguments(
    learning_rate=1e-4,
    num_train_epochs=13,
    per_device_train_batch_size=16,
    per_device_eval_batch_size=16,
    logging_steps=10,
    output_dir="./training_output",
    overwrite_output_dir=False,
    # The next line is important to ensure the dataset labels are properly passed to the model
    remove_unused_columns=True,)

def load_labels():
    with open("labels.json", "r") as labels:
        labels = json.load(labels)
        return labels

def choose_lable(label_preds):
    dict = {'Deliveries': 0,
        'Service_provision': 0,
        'Agreements': 0,
        'Rental': 0,
        'Purchase_and_sale': 0}
    for i in range(len(label_preds)):
        dict[label_preds[i][0]['label']] += 1
    return max(dict, key=dict.get)
    
class Adapter:
    def __init__(self, text: str):
        self.text = text
        self.names = {'Deliveries': '0',
                'Service_provision': '1',
                'Agreements': '2',
                'Rental': '3',
                'Purchase_and_sale': '4'}

    def load_config(self):
        config = RobertaConfig.from_pretrained(
            'models/adapters/adapter_v4/config.json',
            num_labels=2,
        )
        return config

    def load_model(self):
        model = RobertaModelWithHeads.from_pretrained(
            'models/adapters/adapter_v4/pytorch_model.bin',
            config=Adapter.load_config(self)
        )
        model.set_active_adapters("docs")
        return model

    def load_tokenizer(self):
        tokenizer = RobertaTokenizer.from_pretrained('sberbank-ai/ruRoberta-large')
        return tokenizer

    def classify(self):
        classifier = TextClassificationPipeline(model=Adapter.load_model(self), tokenizer=Adapter.load_tokenizer(self))#,
                                                #device=training_args.device.index)
        preds = []
        for k in range(0, len(self.text)//1000):
            preds.append(classifier(self.text[k:k+1000]))
            k += 1000
        return self.names[choose_lable(preds)]


def predict(text: str) -> str:
    names = {'0': 'Договоры поставки',
                '1': 'Договоры оказания услуг',
                '2': 'Договоры подряда',
                '3': 'Договоры аренды',
                '4': 'Договоры купли-продажи'}
    adapter = Adapter(preprocess.preprocess_pipeline(text))
    prediction = adapter.classify()
    return names[prediction] # Output is a label of document(type of a document)
    
