import pandas as pd
import os
import json
from preprocess import preprocess
from preprocess.converter import Converter
from preprocess import preprocess
import pickle
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from sklearn.naive_bayes import MultinomialNB
from sklearn.metrics import f1_score

class Tf_Idf_model:
    def __init__(self):
        self.classifier = MultinomialNB()
        self.vectorizer = TfidfVectorizer()
        self.df = pd.DataFrame()
        self.target_classes = {'Договоры для акселератора/Договоры поставки': '0',
        'Договоры для акселератора/Договоры оказания услуг': '1',
        'Договоры для акселератора/Договоры подряда': '2',
        'Договоры для акселератора/Договоры аренды': '3',
        'Договоры для акселератора/Договоры купли-продажи': '4'}
        self.path = os.getcwd()

    def prepare_data(self):
        with open(f'{self.path}\\ML\\data\\classes.json',  encoding='utf-8') as json_file:
            classes = json.load(json_file)
        
        data = {'id': classes.keys(),
                'label': classes.values()}
        self.df = pd.DataFrame.from_dict(data)
        self.df = self.df.sort_values(by='id')
        self.df = self.df.reset_index()
        del self.df['index']

        for i in range(len(self.df['id'])):
            self.df['id'][i] = self.df['id'][i].split('.')[0] + '.txt'
            self.df['text'] = 0
            self.df['text_norm'] = 0
        texts_id = list(self.df['id'])

        for i in range(len(self.df)):
            with open(f'{self.path}\\data\\txt_data\\' + texts_id[i], 'r', encoding='utf-8') as text:
                text = text.read()
                self.df['text'][i] = text

        for i in range(len(self.df)):
            self.df['text'][i] = self.df['text'][i].replace("Evaluation Only. Created with Aspose.Words. Copyright 2003-2022 Aspose Pty Ltd.", "")
            test_text = preprocess.preprocess_pipeline(self.df['text'][i])
            self.df['text_norm'][i] = test_text

        for i in range(len(self.df)):
            self.df['label'][i] = self.target_classes[self.df['label'][i]]
        return self.df

    def train(self, save_mode=True):
        self.prepare_data()
        X = self.vectorizer.fit_transform(self.df['text_norm']).toarray()
        X_train, self.X_test, y_train, self.y_test = train_test_split(X, self.df['label'], test_size = 0.2,
                                                        random_state = 0)
        self.classifier.fit(X_train, y_train)
        if save_mode:
            with open(f'{self.path}\\models\\tf_idf+mnb.pkl', 'wb') as fid:
                pickle.dump(self.classifier, fid)  
            pickle.dump(self.vectorizer, open(f'{self.path}\\vectorizers\\tf_idf.pickle', 'wb'))  

    def f1_score(self):
        y_pred = self.classifier.predict(self.X_test)
        return f1_score(self.y_test, y_pred, average='macro')
    
    def predict(self, path, model_name=None, vectorizer_name=None):
        if model_name == None:
            self.train(save_mode=True)
        else:
            with open(model_name, 'rb') as fid:
                self.classifier = pickle.load(fid)
            with open(vectorizer_name, 'rb') as f:
                 self.vectorizer = pickle.load(f)

        output_path = f'{self.path}\\test_files'
        new_predict = Converter(path, output_path)
        new_predict.doc()
        text = ''.join(open(f'{self.path}\\test_files\\дог найма Иерусалимская.txt', 'r', encoding = 'utf-8').readlines())
        text = [preprocess.preprocess_pipeline(text)]
        X = self.vectorizer.transform(text).toarray()
        pred = self.classifier.predict(X)
        return pred

        
model = Tf_Idf_model()
path_to_file = 'C:\\Users\\sasha\\PycharmProjects\\AiDoc\\ML\\data\\дог найма Иерусалимская.docx'
print(model.predict(path_to_file, model_name='C:\\Users\\sasha\\PycharmProjects\\AiDoc\\ML\models\\tf_idf+mnb.pkl', vectorizer_name='C:\\Users\\sasha\\PycharmProjects\\AiDoc\\ML\\vectorizers\\tf_idf.pickle'))
