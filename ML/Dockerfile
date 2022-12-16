FROM python:3.9

WORKDIR /code

COPY ./requirements.txt /code/requirements.txt

#RUN apt-get update 
#
#RUN apt-get -y install \
#	python-numpy \
#	python-scipy \
#	libatlas-dev \
#	libatlas3-base
#
##Install scikit-learn
#RUN apt-get -y install python-sklearn

RUN pip install --upgrade pip
RUN pip install --upgrade -r /code/requirements.txt
RUN pip install -U scikit-learn scipy matplotlib
RUN pip3 install torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu

COPY ./ /code
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "80"]