import os
from PyPDF2 import PdfReader
import aspose.words as aw

class Converter:
    def __init__(self, input_path, output_path):
        self.input = input_path
        self.output = output_path
        self.name_of_file = input_path.split('\\')[-1].split('.')[0]

    def pdf(self): #for pdf files
        reader = PdfReader(self.input)
        text = ""
        for page in reader.pages:
            text += page.extract_text() + "\n"
        text_file = open(f'{self.output}\\{self.name_of_file}.txt', "w")
        n = text_file.write(text)
        self._fix_lib_name()
    
    def doc(self): #for rtf/doc/docx files
        doc = aw.Document(self.input)
        doc.save(f'{self.output}\\{self.name_of_file}.txt')

input_path = 'C:\\Users\\sasha\\PycharmProjects\\AiDoc\\ML\\data\\docs'
output_path = "C:\\Users\\sasha\\PycharmProjects\\AiDoc\\ML\\data\\txt_data"

files = os.listdir(input_path)

for file in files:
    if file.split('\\')[-1].split('.')[0] == 'pdf':
        new_predict = Converter(f'{input_path}\\{file}', output_path)
        new_predict.pdf()
    else:
        new_predict = Converter(f'{input_path}\\{file}', output_path)
        new_predict.doc()