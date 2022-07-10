import pytest
import os
import shutil
import stat
from util.containers import start_containers, stop_containers

def onerror(func, path, exc_info):
    if not os.access(path, os.W_OK):
        os.chmod(path, stat.S_IWUSR)
        func(path)
    else:
        raise

@pytest.fixture
def containers():
    git_folder = os.path.abspath(os.path.join(os.getcwd(), os.pardir)) + "/git"
    if not os.path.exists(git_folder):
        os.mkdir(git_folder)
    start_containers()
    yield
    stop_containers()
    shutil.rmtree(git_folder, onerror=onerror)
    os.mkdir(git_folder)
