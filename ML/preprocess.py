import re
from typing import List, Pattern, Callable
import random
import pymorphy2
import string

DUPLICATE_CHARS_TO_REMOVE = ['.', ',', ':', ';', '!', '?', '(', ')', '_', '-', '—', '«', '»', '"']

RE_SPACE_BEFORE_AND_AFTER_WORD = re.compile(r'((?<![\w\s*])(?=\w)|(?<=\w)(?![\w\s*]))', re.MULTILINE)

lemmatizer = pymorphy2.MorphAnalyzer()


def lemmatize(token: str) -> str:
    """
    Lemmatize single token
    :param token: input token
    :return: lemmatized token
    """
    return lemmatizer.parse(token)[0].normal_form


def preprocess_morph(sentence, lemmatization: Callable = lemmatize) -> str:
    """
    Split sentence by whitespace and lemmatize every word
    :param sentence: input string
    :param lemmatization: function that will lemmatize a word
    :return: lemmatized sentence
    """
    return " ".join([lemmatization(token) for token in sentence.split()])


def swap(c: str, i: int, j: int) -> str:
    """
    Swap two characters in a string
    :param c: token
    :param i: first char index
    :param j: second char index
    :return: modified string
    """
    c = list(c)
    c[i], c[j] = c[j], c[i]
    return ''.join(c)


def random_swap(words: List, n: int) -> List:
    """
    Swap tokens in a list
    :param words: list of tokens
    :param n: number of swaps
    :return: new list of tokens
    """
    new_words = words.copy()
    for _ in range(n):
        new_words = swap_word(new_words)
    return new_words


def swap_word(new_words: List) -> List:
    """
    Swap two random words
    :param new_words: list of tokens
    :return: new list of tokens with swap
    """
    random_idx_1 = random.randint(0, len(new_words) - 1)
    random_idx_2 = random_idx_1
    # we shouldn't swap punctuation
    while new_words[random_idx_1] in string.punctuation:
        random_idx_1 = random.randint(0, len(new_words) - 1)
        random_idx_2 = random_idx_1

    counter = 0
    while random_idx_2 == random_idx_1 or new_words[random_idx_2] in string.punctuation:
        random_idx_2 = random.randint(0, len(new_words) - 1)

        counter += 1
        if counter > 7:
            return new_words

    new_words[random_idx_1], new_words[random_idx_2] = new_words[random_idx_2], new_words[random_idx_1]
    return new_words


def add_space_before_and_after_word(input_str: str) -> str:
    """Adds single space before and after word preceded by a punctuation mark.

    Args:
        input_str: Input string.

    Returns:
        String with single spaces between punctuation and words.
    """
    return re.sub(RE_SPACE_BEFORE_AND_AFTER_WORD, ' ', input_str)


def create_pattern_for_duplicated_chars(_char: str) -> Pattern[str]:
    """Creates a pattern to remove all duplicates in text for a character.

    Args:
        _char: Str, the character that the regular expression will be based on.

    Returns:
        Pattern that searches subsequence with duplicate character.
        Ignores empty spaces: ".. .. . ...   .. ." -> "."

    """
    if _char in {'.', '?', '(', ')'}:
        _char = fr'\{_char}'
    pattern = re.compile(fr'{_char}[{_char}\s]*[{_char}]')
    return pattern


def _replace_multiple_characters_with_one(input_str: str, chars: List[str]) -> str:
    """
    Replaces a subsequence with a duplicate characters by one in input string.

    Dynamically creates a matching pattern for each character to replace in the list.
    Ignores empty spaces: ".. .. . ...   .. ." -> "."

    Args:
        input_str: Input string.
        chars: Characters to replace by one in the input_str.

    Returns:
        Input string after replacement.

    """
    for _char in chars:
        pattern = create_pattern_for_duplicated_chars(_char)
        input_str = re.sub(pattern, _char, input_str)

    return input_str


def replace_multiple_characters_with_one(input_str: str) -> str:
    """
    Replacing multiple punctuation marks with one (!!!->!)
    :param input_str: input string
    :return: modified string
    """
    input_str = _replace_multiple_characters_with_one(input_str=input_str, chars=DUPLICATE_CHARS_TO_REMOVE)
    return input_str


def preprocess_pipeline(input_str: str) -> str:
    """
    lowercase, deleting extra white spaces,
    replacing multiple punkt characters with one,
    separating words and punctuation marks with a space
    """
    input_str = input_str.lower().strip()
    input_str = re.sub('ё', 'е', input_str)
    input_str = add_space_before_and_after_word(input_str)
    input_str = replace_multiple_characters_with_one(input_str)
    input_str = ' '.join(input_str.split())
    return input_str
