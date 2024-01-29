import subprocess
import shutil
import os
import docker
import time

client = docker.from_env()

def remove_volume(name):
    try:
        v = client.volumes.get(name)
        v.remove()
    except docker.errors.NotFound:
        pass

def compose_up():
    remove_volume("tech-at-hub_file-store")
    remove_volume("tech-at-hub_database")
    parts = [shutil.which("docker-compose"), "up", "-d"]
    p = subprocess.Popen(parts, cwd=os.getcwd())
    p.wait()
    time.sleep(20)
    assert p.returncode == 0

def compose_down():
    parts = [shutil.which("docker-compose"), "down"]
    p = subprocess.Popen(parts, cwd=os.getcwd())
    p.wait()
    assert p.returncode == 0
    remove_volume("tech-at-hub_file-store")
    remove_volume("tech-at-hub_database")
