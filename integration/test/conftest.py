import pytest
from util.compose import compose_down, compose_up
import time

@pytest.fixture
def compose():
    compose_up()
    time.sleep(3)
    yield
    compose_down()
