import pytest
import os
import shutil
from dotenv import load_dotenv
from util.containers import start_containers, stop_containers

@pytest.fixture
def containers():
    load_dotenv()
    git_folder = os.environ.get("TECH_AT_HUB_PATH") + "/git"
    if not os.path.exists(git_folder):
        os.mkdir(git_folder)
    start_containers()
    yield
    stop_containers()
    shutil.rmtree(git_folder)
    os.mkdir(git_folder)
