import subprocess
import shutil


class GitClient:
    def __init__(self, working_dir):
        self.working_dir = working_dir

    def run(self, command):
        parts = command.split(" ")
        parts[0] = shutil.which("git")
        p = subprocess.Popen(parts, cwd=str(self.working_dir))
        p.wait()
        assert p.returncode == 0
